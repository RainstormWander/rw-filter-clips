**Basic C# script for Streamerbot.**  

Anyone can post clips from the streamer's channel; all other links will be deleted.  
Moderators and VIPs will be able to post any link.  

**Trigger:** Twitch > Chat > Chat Message  
**Sub-action:** Execute Code  

If using this script, replace `'rainstormwander'` with your own channel name:

```csharp
// variable to check for clip syntax for a specific channel
string substringClip = "www.twitch.tv/rainstormwander";
