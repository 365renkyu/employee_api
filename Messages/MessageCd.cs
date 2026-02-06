//余裕あれば作る

using MessageModel;

namespace MessageProcesses
{

    /// <summary>
    /// Messageクラス（リスト型）
    /// 便宜上、DBの代用とする
    /// </summary>
    public class Messages
    {
        public static List<Message> MessageList = new()
            {
                new Message {MessageCd = "E001", CdShubetu = "E", MessageTxt = "既に存在する社員番号です。"},
                new Message {MessageCd = "E002", CdShubetu = "E", MessageTxt = "登録に失敗しました。"},
                new Message {MessageCd = "E003", CdShubetu = "E", MessageTxt = "社員が存在しません。"}
            };

    };

    /*
        /// <summary>
        /// メッセージ表示関連処理のクラス
        /// </summary>
        public class MessageProcess
        {
            public string DispMessageTxt(string messageCd)
            {
                var list = new Messages.MessageList;

                return txt;
            }

        }
        */

}
