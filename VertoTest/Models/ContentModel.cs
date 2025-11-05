using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VertoTest.Models;

public class ContentModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Text { get; set; } = "";

    public string Text2 { get; set; } = "";

    public string Name { get; set; } = "";

    public string PhotoName { get; set; } = "";


    public ContentModel(int id, string text, string text2, string name, string photoName)
    {
        Id = id;
        Name = name;
        Text = text;
        Text2 = text2;
        PhotoName = photoName;

    }

    public ContentModel()
    { }


}

