using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using Twitterizer;

namespace Whisper
{
    public class TwitterUtil
    {
        OAuthTokens _tokens = new OAuthTokens();

        public TwitterUtil()
        {
            _tokens.AccessToken = Whisper.Properties.Settings.Default.AccessToken;
            _tokens.AccessTokenSecret = Whisper.Properties.Settings.Default.AccessTokenSecret;
            _tokens.ConsumerKey = Whisper.Properties.Settings.Default.ConsumerKey;
            _tokens.ConsumerSecret = Whisper.Properties.Settings.Default.ConsumerSecret;
        }
        public void RequestGetDirectMessage(Action<TwitterDirectMessageCollection> action)
        {
            DirectMessagesOptions option = new DirectMessagesOptions();
            option.APIBaseAddress = Whisper.Properties.Settings.Default.APIBaseAddress;

            TwitterResponse<TwitterDirectMessageCollection> response = TwitterDirectMessage.DirectMessages(_tokens, option);
            if (response.Result == RequestResult.Success)
            {
                action(response.ResponseObject);
            }
        }
        public void RequestGetDirectMessageSent(Action<TwitterDirectMessageCollection> action)
        {
            DirectMessagesSentOptions option = new DirectMessagesSentOptions();
            option.APIBaseAddress = Whisper.Properties.Settings.Default.APIBaseAddress;

            TwitterResponse<TwitterDirectMessageCollection> response = TwitterDirectMessage.DirectMessagesSent(_tokens, option);
            if (response.Result == RequestResult.Success)
            {
                action(response.ResponseObject);
            }
        }
        public void RequestGetDirectMessageNew(string screen_name, string text, Action<TwitterDirectMessage> action)
        {
            OptionalProperties option = new OptionalProperties();
            option.APIBaseAddress = Whisper.Properties.Settings.Default.APIBaseAddress;

            TwitterResponse<TwitterUser> showUserResponse = TwitterUser.Show(_tokens, screen_name, option);
            if (showUserResponse.Result == RequestResult.Success)
            {
                TwitterResponse<TwitterDirectMessage> response = TwitterDirectMessage.Send(_tokens, showUserResponse.ResponseObject.Id, text, option);
                if (response.Result == RequestResult.Success)
                {
                    action(response.ResponseObject);
                }
            }

        }
    }
}
