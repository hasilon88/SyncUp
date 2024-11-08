using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song 
{
    public string ID { get; set; }
    public string Title { get; set; }
    public int Duration { get; set; }
    public string ArtistID { get; set; }
    public string ArtistName { get; set; }
    public string AlbumID { get; set; }
    public string AlbumName { get; set; }
    public bool IsPlayable { get; set; }
    public bool IsLocal { get; set; }

    public override string ToString()
    {
        return $"ID: {ID}\n" +
               $"Title: {Title}\n" +
               $"Duration: {Duration} seconds\n" +
               $"Artist ID: {ArtistID}\n" +
               $"Artist Name: {ArtistName}\n" +
               $"Album ID: {AlbumID}\n" +
               $"Album Name: {AlbumName}\n" +
               $"Is Playable: {IsPlayable}\n" +
               $"Is Local: {IsLocal}";
    }
}
