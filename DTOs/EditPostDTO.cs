namespace API.DTOs
{
    public class EditPostDTO
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }

        public EditPostDTO(){
            if(PostTitle == null){
                PostTitle = "";
            }
            if(PostContent == null){
                PostContent = "";
            } 
        }
    }
}