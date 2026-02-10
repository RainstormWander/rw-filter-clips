using System;
// system that lets us use variable.Contains()
using System.Collections.Generic;

public class CPHInline
{
    // - Function Declarations - //

    // Purpose: get messages inputs (user, message ID, chat message text content)
    public (string, string, string) GetMessageInputs()
    {
        // get text content
        string rawInput = args["rawInput"].ToString();
        // debug message - only enable for testing, it will run forever
        // CPH.SendMessage($"Raw input: {rawInput}");

        // get message ID
        string messageID = args["msgId"].ToString();
        // debug message - only enable for testing, it will run forever
        // CPH.SendMessage($"Message ID: {messageID}");
        
        // initialize user id variable
        string userID = "";

        // get userId of the person who triggered the action
        if (args.TryGetValue("userId", out object userIDObject))
            {
            userID = userIDObject.ToString();
            // debug message - only enable for testing, it will run forever
            // CPH.SendMessage($"User ID: {userID}"); 
            }
        else
            {
            // debug message - only enable for testing, it will run forever
            // CPH.SendMessage($"Unable to retrieve User ID.");
            }
        
        // return text content, message ID, and username as strings
        return (rawInput, messageID, userID);
    }

    // Purpose: validate whether user is permitted to post links
    public bool IsUserPermittedToPostLinks(string userID) {

        // use built-in function to fetch user info based on userID
        var userInfo = CPH.TwitchGetUserInfoById(userID);
        // track whether user is permitted to post links (ie. a moderator or vip)
        bool userPermitted = userInfo.IsModerator || userInfo.IsVip;
        // return result of user validation check
        return userPermitted;
    }

    // Purpose: check raw input for presence of link
    public bool ValidateSubstring(string rawInput, string messageID, string userID)  {

        // variables to check for "www." or "http" in the chat message
        string substringLinkSyntaxWWW = "www.";
        string substringLinkSyntaxHTTP = "http";
        // bool to track whether a link has been found
        bool linkFound = false;

        // if the chat message contains link syntax
        if (rawInput.Contains(substringLinkSyntaxWWW) || (rawInput.Contains(substringLinkSyntaxHTTP)))
        {
            // sent linkFound to true
            linkFound = true;
            // send debug message to chat
            // CPH.SendMessage($"Link: {linkFound}");
            // call function to validate whether link is a valid clip
            bool validLink = ValidateLinkDetails(rawInput);
            // call function to validate whether user is permitted to post links
            bool userPermitted = IsUserPermittedToPostLinks(userID);          
            // call function to delete link if required
            DeleteLink(validLink, userPermitted, messageID);
            // return true when all functions are done executing
            return true;
        }
        // if link syntax has not been found, return false
        else {
            // debug message - only enable for testing, it will run forever
            // CPH.SendMessage($"Link: {linkFound}");
            return false;
        }
    }

    // Purpose: validate whether link is permitted
    public bool ValidateLinkDetails(string rawInput) {
        // variable to check for clip syntax for a specific channel
        string substringClip = "www.twitch.tv/rainstormwander";
        // to track whether a valid link has been found
        bool validLink = false;

        // if the raw input contains clip syntax substring
        if (rawInput.Contains(substringClip)) {
            // set validLink to true
            validLink = true;
            // debug message
            // CPH.SendMessage($"Valid link: {validLink}");
            // return true
            return true;
        }
        // if the link is not a clip
        else {
            // debug message
            // CPH.SendMessage($"Valid link: {validLink}");
            return false;
    }
    }

    // Purpose: delete link if it's not a valid link
    public bool DeleteLink(bool validLink, bool userPermitted, string messageID) {
        
        // if we've found a valid link or valid user, leave the message with the link in chat
        if (validLink || userPermitted) {
            // debug message
            // CPH.SendMessage($"Valid link: {validLink} | Valid user: {userPermitted}. Leaving link in chat...");
            return true;
        }
        // if we haven't found a valid link or user
        else {
            // delete link using built-in streamerbot function
            CPH.TwitchDeleteChatMessage(messageID, false);
            // debug message
            // CPH.SendMessage($"Valid link: {validLink} | Valid user: {userPermitted}. Leaving link in chat...");
            return false;
            }        
        }
    
    // - Function Calls - //

    // function that executes when action triggers
    public bool Execute()
    {
        // get the raw inputs from the chat message
        (string rawInput, string messageID, string userID) = GetMessageInputs();
        // run input validation checks and delete link if necessary
        return ValidateSubstring(rawInput, messageID, userID);
    }
}