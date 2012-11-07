using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Whisper
{

    public class Url
    {
        public string url { get; set; }
        public string expanded_url { get; set; }
        public string display_url { get; set; }
        public int[] indices { get; set; }
    }

    public class Entities
    {
        public Url[] urls { get; set; }
    }

    public class Metadata
    {
        public int recent_retweets { get; set; }
        public string result_type { get; set; }
    }

    [DataContract]
    public class Result
    {
        [DataMember]
        public string created_at { get; set; }

        [DataMember]
        public Entities entities { get; set; }

        [DataMember]
        public string from_user { get; set; }

        [DataMember]
        public int from_user_id { get; set; }

        [DataMember]
        public string from_user_id_str { get; set; }

        [DataMember]
        public object geo { get; set; }

        [DataMember]
        public object id { get; set; }

        [DataMember]
        public string id_str { get; set; }

        [DataMember]
        public string iso_language_code { get; set; }

        [DataMember]
        public Metadata metadata { get; set; }

        [DataMember]
        public string profile_image_url { get; set; }

        [DataMember]
        public string source { get; set; }

        [DataMember]
        public string text { get; set; }

        [DataMember]
        public object to_user_id { get; set; }

        [DataMember]
        public object to_user_id_str { get; set; }
    }

    [DataContract]
    public class RootObject
    {
        [DataMember]
        public double completed_in { get; set; }

        [DataMember]
        public long max_id { get; set; }

        [DataMember]
        public string max_id_str { get; set; }

        [DataMember]
        public string next_page { get; set; }

        [DataMember]
        public int page { get; set; }

        [DataMember]
        public string query { get; set; }

        [DataMember]
        public string refresh_url { get; set; }

        [DataMember]
        public Result[] results { get; set; }

        [DataMember]
        public int results_per_page { get; set; }

        [DataMember]
        public int since_id { get; set; }

        [DataMember]
        public string since_id_str { get; set; }
    }

    [DataContract]
    public class DirectMessageModel : INotifyPropertyChanged
    {
        private object created_atValue;

        [DataMember]
        public object created_at
        {
            get { return created_atValue; }
            set
            {
                if (value != created_atValue)
                {
                    created_atValue = value;
                    RaisePropertyChanged("created_at");
                }
            }
        }

        private object sender_idValue;

        [DataMember]
        public object sender_id
        {
            get { return sender_idValue; }
            set
            {
                if (value != sender_idValue)
                {
                    sender_idValue = value;
                    RaisePropertyChanged("sender_id");
                }
            }
        }


        private object sender_screen_nameValue;

        [DataMember]
        public object sender_screen_name
        {
            get { return sender_screen_nameValue; }
            set
            {
                if (value != sender_screen_nameValue)
                {
                    sender_screen_nameValue = value;
                    RaisePropertyChanged("sender_screen_name");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var h = this.PropertyChanged;
            if (h != null)
            {
                h(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
