using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Methods {
    public class BannedWordMethod {

        public async Task OnBannedWords(DiscordGuild guild, DiscordMessage message) {

            var staffLogCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            if (staffLogCh == null) {
                return;
            }

            List<string> bannedWords = new List<string>() { "faggot", "nigger", "nigga", "cunt", "retard", "spastic", "rape", "whore", "slut", "cum", "anal", "blowjob", "fagot", "retarded" };

            bool bWord = false;

            string usedWord = string.Empty;

            string userInput = message.Content.ToLower();
            string[] wordsInMessage = userInput.Split(new char[] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (userInput.Count() < 2) {
                foreach (var word in bannedWords) {
                    if (userInput == word) {

                        usedWord = word;

                        bWord = true;
                        continue;
                    }
                }
            }
            else {

                foreach (var word in bannedWords) {

                    if (wordsInMessage.Contains(word)) {

                        usedWord = word;

                        bWord = true;
                        continue;

                    }
                }
            }

            if (bWord) {

                await message.Channel.DeleteMessageAsync(message).ConfigureAwait(false);

                await staffLogCh.SendMessageAsync($"{message.Author.Mention} just tried to use a blacklisted word in {message.Channel.Mention}. The message was instantly deleted.").ConfigureAwait(false);

            }

        }

    }
}
