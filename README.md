# Deck Bot for Twitch

This plugin has an integrated chat bot for Twitch in order to be able to dynamically post the deck code of your currently active deck so you don't have to update your chat commands everytime you switch decks. This is for people who don't want to or aren't able to use the HDT Twitch extension.

## Usage
First you need to specifiy a channel name, the Twitch account name for the bot and an authentication token (OAuth) in the plugin settings. When you're good to go, go to Plugins > Deck Bot and let the bot connect to your chat. As long as the bot is connected, it's going to respond to the specified chat command with the deck code of the currently active deck (1.5 sec. cooldown).

### Generating an OAuth token
To generate a token, login to the Twitch account you want to use as the bot and visit https://twitchapps.com/tmi/. Copy the generated token into the `OAuth token` field in the plugin settings and you're done.

## Used Resources
* Status icons made by [RiotRob](https://twitch.tv/RiotRob)
