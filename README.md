IRCBot
======

IRCBot is a C# bot that uses a plugin-based system. Currently the project has 4 plugins:
- Administrator (bot management via IRC)
- ChatBot (CleverBot conversations)
- DictionaryLookup (urban-dictionary based)
- ChannelManager (keeps track of which channels the bot is on and allows the bot to be commanded to join channels).

It provides a GUI as a way of keeping track of the messages the bot sends and recieves; however, any implementation of IPluginHost can be used to run the plugins that provide the actual functionality of the bot.