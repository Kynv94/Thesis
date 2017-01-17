using System;
using System.Text;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.IpV6;
using PcapDotNet.Packets.Arp;
using PcapDotNet.Packets.Http;
using System.Net;
using PcapDotNet.Packets.Transport;
using PcapDotNet.Packets;
using WpfApplication1.Database;
using PcapDotNet.Packets.Icmp;
using System.Linq;
using System.Collections.Generic;

namespace WpfApplication1
{
    class HandlePacket
    {
        public static void add_informationv2(Packet packet)
        {
            //Timestamp, MAC
            Session new_session = new Session();
            Detail new_detail = new Detail();
            new_session.Started = packet.Timestamp;
            new_session.Ended = packet.Timestamp;
            new_session.MAC_in = packet.Ethernet.Source.ToString() ?? "00:00:00:00:00:00";
            new_session.MAC_out = packet.Ethernet.Destination.ToString() ?? "00:00:00:00:00:00";
            switch (packet.Ethernet.EtherType.GetHashCode())
            {
                //Ipv4
                case 2048:
                    {
                        add_new_packet(new_session, new_detail, packet.Ethernet.IpV4);
                        break;
                    }
                //Address Resolution Protocol
                case 2054:
                    {
                        add_new_packet(new_session, new_detail, packet.Ethernet.Arp);
                        break;
                    }
                //Ipv6
                case 34525:
                    {
                        add_new_packet(new_session, new_detail, packet.Ethernet.IpV6);
                        break;
                    }
                default:
                    {
                        new_session = null;
                        new_detail = null;
                        break;
                    }
            }
            HandleDatabase db = new HandleDatabase();
            db.Add_dataV2(new_session, new_detail);
        }
        //add information cac goi Ipv4
        private static void add_new_packet(Session new_session, Detail new_detail, IpV4Datagram ipv4_packet)
        {
            HandleDatabase db = new HandleDatabase();
            //add IP, Port, SSL, State, Detail
            //IP_in
            new_session.IP_in = ipv4_packet.Source.ToString() ?? "0.0.0.0";
            new_session.IP_in_is_v4 = true;
            //IP_out
            new_session.IP_out = ipv4_packet.Destination.ToString() ?? "0.0.0.0";
            new_session.IP_out_is_v4 = true;
            if (new_session.IP_in == new_session.IP_out && new_session.IP_in == "0.0.0.0")
            {
                new_session = null;
                new_detail = null;
            }
            else
            {
                //Source Port
                try
                {
                    new_session.Port_in = ipv4_packet.Transport.SourcePort;
                }
                catch (Exception)
                {
                    new_session.Port_in = 0;
                }
                //Destination port
                try
                {
                    new_session.Port_out = ipv4_packet.Transport.DestinationPort;
                }
                catch (Exception)
                {
                    new_session.Port_out = 0;
                }
                switch (ipv4_packet.Protocol.GetHashCode()) //add SSL, state, Detail 
                {
                    //Tcp
                    case 6:
                        {
                            if (ipv4_packet != null)
                            {
                                TcpDatagram tcp_packet = ipv4_packet.Tcp;
                                //add session

                                //state - new thread
                                Session oldsession = db.get_old_session(new_session);
                                new_session.State = 0; //unknown state

                                int oldsession_state;
                                if (oldsession == null)
                                    oldsession_state = 0;
                                else
                                    oldsession_state = oldsession.State;
                                //Add state to new session
                                add_state(new_session, oldsession_state, tcp_packet);

                                //SSL - newthread
                                if (ipv4_packet.Transport.SourcePort == 443 || ipv4_packet.Transport.DestinationPort == 443)
                                    new_session.IsSSL = true;
                                else
                                    new_session.IsSSL = false;

                                //Detail - New thread
                                add_detail(new_detail, new_session, ipv4_packet.Tcp);
                            }
                            else
                            {
                                new_session = null;
                                new_detail = null;
                            }
                            break;
                        }

                    //InternetControlMessageProtocol
                    case 1:
                        {
                            if (ipv4_packet.Icmp != null)
                            {
                                new_session.IsSSL = false;
                                new_session.State = 4;
                                add_detail(new_detail, new_session, ipv4_packet.Icmp);
                            }
                            else
                            {
                                new_session = null;
                                new_detail = null;
                            }
                            break;
                        }
                    //Udp
                    case 17:
                        {
                            if (ipv4_packet.Udp != null)
                            {
                                //SSL - newthread
                                if (ipv4_packet.Transport.SourcePort == 443 || ipv4_packet.Transport.DestinationPort == 443)
                                    new_session.IsSSL = true;
                                else
                                    new_session.IsSSL = false;
                                //State
                                new_session.State = 4;
                                //new thread
                                add_detail(new_detail, new_session, ipv4_packet.Udp);
                            }
                            else
                            {
                                new_session = null;
                                new_detail = null;
                            }
                            break;
                        }

                    //InternetGroupManagementProtocol
                    //case 2:
                    //    {
                    //        if (ipv4_packet.Igmp.IsValid)
                    //        {
                    //            new_session.State = 4;
                    //            new_session.IsSSL = false;
                    //            //newthread
                    //            new_detail.UpdateTime = new_session.Started;
                    //            add_detail(new_detail, new_session, ipv4_packet.Igmp);
                    //        }
                    //        else
                    //        {
                    //            new_session = null;
                    //            new_detail = null;
                    //        }
                    //        break;
                    //    }

                    //Generic Routing Encapsulation
                    //case 47: break;
                    default:
                        {
                            new_session.IsSSL = false;
                            new_session.State = 4; //done
                                                   //Detail
                            new_detail = null;
                            break;
                        }
                }
            }
        }
        //TCP state cua Ipv4
        private static void add_state(Session new_session, int old_session_state, TcpDatagram tcp_packet)
        {
            switch (old_session_state)
            {
                case 0: //unknow state
                    {
                        if (!tcp_packet.IsAcknowledgment)
                        {
                            if (tcp_packet.IsSynchronize && !tcp_packet.IsPush && !tcp_packet.IsReset && !tcp_packet.IsFin && !tcp_packet.IsUrgent)
                                new_session.State = 1; //Asked to connect
                            else
                                new_session.State = old_session_state; //Unknown state
                        }
                        else
                        {
                            new_session.State = old_session_state; //Unkown state
                        }
                        break;
                    }
                case 1: //asked to connect
                    {
                        if (tcp_packet.IsAcknowledgment)
                        {
                            if (tcp_packet.IsSynchronize && !tcp_packet.IsPush && !tcp_packet.IsReset && !tcp_packet.IsFin && !tcp_packet.IsUrgent)
                                new_session.State = 2; //respond to connect 
                            else
                            {
                                if (!tcp_packet.IsSynchronize && !tcp_packet.IsPush && tcp_packet.IsReset && !tcp_packet.IsFin && !tcp_packet.IsUrgent)
                                    new_session.State = 4; //Finished
                                else
                                    new_session.State = old_session_state; //asked to connect
                            }
                        }
                        else
                        {
                            new_session.State = old_session_state; //Asked to connect
                        }
                        break;
                    }
                case 2: //responded
                    {
                        if (tcp_packet.IsAcknowledgment)
                        {
                            //
                            if (!tcp_packet.IsSynchronize && !tcp_packet.IsPush && !tcp_packet.IsReset && !tcp_packet.IsFin && !tcp_packet.IsUrgent)
                                new_session.State = 3; //connected
                            else
                            {
                                if (!tcp_packet.IsSynchronize && !tcp_packet.IsPush && tcp_packet.IsReset && !tcp_packet.IsFin && !tcp_packet.IsUrgent)
                                    new_session.State = 4; //Finished
                                else
                                    new_session.State = old_session_state; //Responed
                            }
                        }
                        else
                        {
                            new_session.State = old_session_state; //Responed
                        }
                        break;
                    }
                case 3: //Connected
                    {
                        if (tcp_packet.IsAcknowledgment)
                        {
                            if (!tcp_packet.IsSynchronize && !tcp_packet.IsPush && !tcp_packet.IsReset && tcp_packet.IsFin && !tcp_packet.IsUrgent)
                                new_session.State = 5; //A Disconected and B doesnt know that
                            else
                            {
                                if (!tcp_packet.IsSynchronize && !tcp_packet.IsPush && tcp_packet.IsReset && !tcp_packet.IsFin && !tcp_packet.IsUrgent)
                                    new_session.State = 4; //Finished
                                else
                                    new_session.State = old_session_state; //Connected
                            }
                        }
                        else
                        {
                            new_session.State = old_session_state; //Connected
                        }
                        break;
                    }
                case 4: //Finished
                    {
                        old_session_state = 0;
                        if (!tcp_packet.IsAcknowledgment)
                        {
                            if (tcp_packet.IsSynchronize && !tcp_packet.IsPush && !tcp_packet.IsReset && !tcp_packet.IsFin && !tcp_packet.IsUrgent)
                                new_session.State = 1; //Asked to connect
                            else
                                new_session.State = old_session_state; //Finished
                        }
                        else
                        {
                            new_session.State = old_session_state; //Finished
                        }
                        break;
                    }
                case 5: //A disconected and B doesnt know that
                    {
                        if (tcp_packet.IsAcknowledgment)
                        {
                            if (!tcp_packet.IsSynchronize && !tcp_packet.IsPush && !tcp_packet.IsReset && !tcp_packet.IsFin && !tcp_packet.IsUrgent)
                                new_session.State = 6; //A disconected and B knows A that
                            else
                                new_session.State = old_session_state; //B doesnt know A disconnected
                        }
                        else
                        {
                            new_session.State = old_session_state; //B doesnt know A disconnected
                        }
                        break;
                    }
                case 6: //B knows A disconected
                    {
                        if (tcp_packet.IsAcknowledgment)
                        {
                            if (!tcp_packet.IsSynchronize && !tcp_packet.IsPush && !tcp_packet.IsReset && tcp_packet.IsFin && !tcp_packet.IsUrgent)
                                new_session.State = 7; //B Disconnected
                            else
                                new_session.State = old_session_state; //A disconected and B knows that
                        }
                        else
                        {
                            new_session.State = old_session_state; //A disconected and B knows that
                        }
                        break;
                    }
                case 7: //B Disconected and A doesnt know that
                    {
                        if (tcp_packet.IsAcknowledgment)
                        {
                            if (!tcp_packet.IsSynchronize && !tcp_packet.IsPush && !tcp_packet.IsReset && tcp_packet.IsFin && !tcp_packet.IsUrgent)
                                new_session.State = 4; //B Disconected and A knows that
                            else
                                new_session.State = old_session_state; //B disconected and A doesnt know that
                        }
                        else
                        {
                            new_session.State = old_session_state; //B disconected and A doesent know that
                        }
                        break;
                    }
                default: new_session.State = old_session_state; break;
            }
        }
        //Detail cua cacs goi TCP
        private static void add_detail(Detail new_detail, Session new_session, TcpDatagram tcp_packet)
        {
            //UpdateTime
            new_detail.UpdateTime = new_session.Started;
            //PluginID
            try
            {
                new_detail.PluginID = tcp_packet.SourcePort < tcp_packet.DestinationPort ? tcp_packet.SourcePort : tcp_packet.DestinationPort;
            }
            catch (Exception)
            {
                new_detail.PluginID = 0;
            }
            //BinData
            new_detail.BinData = tcp_packet.Payload.ToHexadecimalString() ?? string.Empty;
            //KeyData, TextData
            detail_tcp_data(new_detail, new_session, tcp_packet);
        }
        private static void detail_tcp_data(Detail new_detail, Session new_session, TcpDatagram tcp)
        {
            //Keydata, TextData
            switch (new_detail.PluginID)
            {
                case 80:
                    {
                        if (tcp.Http.Header == null)
                        {
                            //http khong hop le
                            break;
                        }
                        if (tcp.Http.IsRequest)
                        {
                            HttpRequestDatagram request = (HttpRequestDatagram)tcp.Http;

                            //TextData
                            string requesttext = request.Method.Method.ToString() + request.Uri.ToString()
                            + " " + request.Version.ToString() + "\r\n" + request.Header.ToString() ?? string.Empty;

                            new_detail.TextData = requesttext ?? string.Empty;

                            //KeyData
                            //if (request.Header != null)
                            try
                            {
                                string str = request.Header.ToString().ToLower();
                                if (str.Contains("referer:"))
                                {
                                    var index1 = str.IndexOf("referer:");
                                    var index2 = str.IndexOf(' ', index1);
                                    new_detail.KeyData = str.Substring(index2 + 1, str.IndexOf(Environment.NewLine, index2 + 1) - index2) ?? string.Empty;
                                    break;
                                }
                                if (str.Contains("host:"))
                                {
                                    var index1 = str.IndexOf("host:");
                                    var index2 = str.IndexOf(' ', index1);
                                    new_detail.KeyData = str.Substring(index2 + 1, str.IndexOf(Environment.NewLine, index2 + 1) - index2) ?? string.Empty;
                                    break;
                                }

                            }
                            //else
                            catch (Exception)
                            {
                                new_detail.KeyData = request.Uri ?? string.Empty;
                            }
                            break;
                        }
                        else
                        {

                            HttpResponseDatagram respond = (HttpResponseDatagram)tcp.Http;

                            //TextData
                            string respondtext = (respond.Version.ToString() + " " + respond.StatusCode
                            + " " + respond.ReasonPhrase.Decode(Encoding.UTF8)
                            + "\r\n" + respond.Header.ToString()) ?? string.Empty;

                            new_detail.TextData = respondtext ?? string.Empty;

                            //Keydata
                            try
                            {
                                string str = respond.Header.ToString().ToLower();
                                if (str.Contains("Server"))
                                {
                                    var index = str.IndexOf(' ');
                                    new_detail.KeyData = respond.Header.ToString().Substring(index + 1, str.IndexOf(Environment.NewLine) - index) ?? string.Empty;
                                    break;
                                }
                                else
                                    new_detail.KeyData = string.Empty;
                            }
                            catch (Exception)
                            {
                                new_detail.KeyData = string.Empty;
                            }

                            break;
                        }
                    }
                case 443:
                    {
                        //TextData Keydata
                        try
                        {
                            new_detail.KeyData = Dns.GetHostEntry(new_session.IP_out).HostName;
                        }
                        catch (Exception)
                        {
                            new_detail.KeyData = string.Empty;
                        }
                        new_detail.TextData = string.Empty;
                        break;
                    }

                default:
                    {
                        //KeyData
                        new_detail.KeyData = string.Empty;
                        //TextData
                        new_detail.TextData = string.Empty;
                        break;
                    }
            }
        }
        //Detail cua cac goi UDP
        private static void add_detail(Detail new_detail, Session new_session, UdpDatagram udp_packet)
        {
            //UpdateTime
            new_detail.UpdateTime = new_session.Started;
            //PluginID
            try
            {
                new_detail.PluginID = udp_packet.SourcePort < udp_packet.DestinationPort ? udp_packet.SourcePort : udp_packet.DestinationPort;
            }
            catch (Exception)
            {
                new_detail.PluginID = 0;
            }
            //TextData and KeyData
            new_detail.KeyData = string.Empty;
            if (udp_packet.Dns.IsValid || new_detail.PluginID == 53)
            {
                new_detail.PluginID = 53;
                //KeyData
                if (udp_packet.Dns.IsQuery)
                {
                    new_detail.KeyData = "DNS Query";
                }
                if (udp_packet.Dns.IsResponse)
                {
                    new_detail.KeyData = "DNS Respond";
                }
                string tmp = udp_packet.Dns.Decode(Encoding.ASCII) ?? string.Empty;
                if (tmp != null)
                {
                    new_detail.TextData = null;
                    foreach (char i in tmp)
                    {
                        if (i >= 40 && i <= 176)
                            new_detail.TextData += i;
                    }
                }
            }
            else
            {
                //KeyData
                try
                {
                    new_detail.KeyData = Dns.GetHostEntry(new_session.IP_out).HostName;
                }
                catch (Exception)
                {
                    new_detail.KeyData = string.Empty;
                }
                //textdata
                new_detail.TextData = string.Empty;
            }
            //BinData
            //new_detail.BinData = udp_packet.ToHexadecimalString() ?? string.Empty;
            new_detail.BinData = string.Empty;
        }
        //Detail cua cac goi IGMP
        //private static void add_detail(Detail new_detail, Session new_session, IgmpDatagram igmp)
        //{
        //    //UpdateTime
        //    new_detail.UpdateTime = new_session.Started;
        //    //PluginID
        //    new_detail.PluginID = 2; //igmp
        //    //Keydata
        //    new_detail.KeyData = "Internet Group Management Protocol"; igmp.
        //    //TextData
        //    new_detail.TextData = igmp.MessageType.ToString() ?? string.Empty;
        //    //BinData
        //    // new_detail.BinData = igmp.ToHexadecimalString();
        //    new_detail.BinData = string.Empty;
        //}
        //Detail cua cac goi ICMP
        private static void add_detail(Detail new_detail, Session new_session, IcmpDatagram icmp)
        {
            //UpdateTime
            new_detail.UpdateTime = new_session.Started;
            //PluginID
            new_detail.PluginID = 1; //ICMP
            //Keydata
            new_detail.KeyData = icmp.MessageType.ToString() ?? string.Empty; ;
            //TextData
            new_detail.TextData = icmp.MessageTypeAndCode.ToString() ?? string.Empty;
            //BinData
            if (icmp.Payload != null)
                new_detail.BinData = icmp.Payload.Decode(Encoding.UTF8);
            else
                new_detail.BinData = string.Empty;
        }
        //add information cac goi Ipv6
        private static void add_new_packet(Session new_session, Detail new_detail, IpV6Datagram ipv6_packet)
        {
            //Session: IP, Port, SSL, State

            //IP Address
            new_session.IP_in = ipv6_packet.Source.ToString() ?? "::";
            new_session.IP_in_is_v4 = false;
            new_session.IP_out = ipv6_packet.CurrentDestination.ToString() ?? "::";
            new_session.IP_out_is_v4 = false;
            if (new_session.IP_in == new_session.IP_out && new_session.IP_in == "::")
            {
                new_session = null;
                new_detail = null;
            }
            else
            {
                //Port
                try
                {
                    new_session.Port_in = ipv6_packet.Transport.SourcePort;
                }
                catch (Exception)
                {
                    new_session.Port_in = 0;
                }
                try
                {
                    new_session.Port_out = ipv6_packet.Transport.DestinationPort;
                }
                catch (Exception)
                {
                    new_session.Port_out = 0;
                }

                new_session.IsSSL = false;
                new_session.State = 4;

                //Details
                new_detail = null;
            }
        }
        //add information cac goi Arp
        private static void add_new_packet(Session new_session, Detail new_detail, ArpDatagram arp_packet)
        {
            //Session: IP, Port, SSL, State
            new_session.IP_in = arp_packet.SenderProtocolIpV4Address.ToString() ?? "0.0.0.0";
            new_session.IP_in_is_v4 = true;
            new_session.IP_out = arp_packet.TargetProtocolIpV4Address.ToString() ?? "0.0.0.0";
            new_session.IP_out_is_v4 = true;
            if (new_session.IP_in == new_session.IP_out && new_session.IP_in == "0.0.0.0")
            {
                new_session = null;
                new_detail = null;
            }
            else
            {
                new_session.Port_in = 0;
                new_session.Port_out = 0;
                new_session.IsSSL = false;
                new_session.State = 4;

                //Details
                new_detail = null;
            }
        }

        internal List<AtlertResult> AtlertResults()
        {
            var hb = new HandleDatabase();
            var ListResult = new List<AtlertResult>();
            foreach (var item in hb.get_alert_web())
            {
                var compareResult = hb.AlertCompare(item.Address);
                if (compareResult != null)
                {
                    var result = new AtlertResult();
                    result.Date = compareResult.UpdateTime;
                    result.Name = item.Alert.AlertName;
                    result.EventID = compareResult.Det_ID;
                    result.IP_Source = compareResult.Session.IP_in;
                    ListResult.Add(result);
                }
            }
            return ListResult;
        }

    }
}
