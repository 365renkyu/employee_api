using System.ComponentModel.DataAnnotations;

namespace MessageModel
{
    public class Message
    {
        [Required]
        [StringLength(4)]
        public string MessageCd = ""; //メッセージコード

        [Required]
        [StringLength(2)]
        public string CdShubetu = ""; //コード種別（M：メッセージ、W：警告、E：エラー）

        [Required]
        [StringLength(30)]
        public string MessageTxt = ""; //内容
    };
}