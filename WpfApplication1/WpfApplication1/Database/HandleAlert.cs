using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Database
{
    class HandleAlert
    {
        //ADD
        internal static Alert add_alert(string name, bool enable, bool anounce, bool popup)
        {
            var hb = new HandleDatabase();
            var new_alert = new Alert();
            new_alert.AlertName = name;
            new_alert.Enable = enable;
            new_alert.Anouncement = anounce;
            new_alert.Popup = popup;
            hb.add_alert(new_alert);
            return new_alert;
        }

        internal static void add_web(string address, Alert _alert)
        {
            var hb = new HandleDatabase();
            var new_web_alert = new AlertWeb();
            new_web_alert.Address = address;
            hb.add_event(new_web_alert, _alert);
        }

        //MODIFY
        internal static void modify_alert(int _alertID, string name, bool enable, bool anounce, bool popup)
        {
            var hb = new HandleDatabase();
            var _alert = hb.get_alert(_alertID);
            _alert.AlertName = name;
            _alert.Enable = enable;
            _alert.Anouncement = anounce;
            _alert.Popup = popup;
            hb.modify_alert(_alert);
        }
        internal static void modify_web_entry(int _webID, string address)
        {
            var hb = new HandleDatabase();
            var _web = hb.get_alert_web(_webID);
            _web.Address = address;
        }

        //DELETE
        internal static void delete_alert(int _alertID)
        {
            var hb = new HandleDatabase();
            hb.delete_alert(_alertID);
        }
        internal static void delete_web(int _webID)
        {
            var hb = new HandleDatabase();
            hb.delete_web(_webID);
        } 

    }
}
