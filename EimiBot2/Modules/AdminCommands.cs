using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using Discord.Addons.Interactive;
using MySql.Data.MySqlClient.Memcached;
using Google.Apis.Discovery;

namespace EimiBot2.Modules
{
    public class AdminCommands : InteractiveBase
    {
        private readonly Logger log = new Logger();
        public bool ReplyStatus;
        public string Prf;
        private string datasource;
        private string dbname;
        private string user;
        private string pass;

        private void ObtainDatabaseInfo()
        {
            string[] lines;

            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\mysql.config");

            lines = file.ReadLine().Split('='); 
            datasource = lines[1];
            lines = file.ReadLine().Split('=');
            dbname = lines[1];
            lines = file.ReadLine().Split('=');
            user = lines[1];
            lines = file.ReadLine().Split('=');
            pass = lines[1];

            log.Debug($"Data Source: {datasource}; DB Name: {dbname}; User: {user};");

            file.Close();
            log.Info("Database info successfully retrieved.");
        }

        public bool GetInput(string response)
        {
            log.Info("Prefix Response:" + response.ToString());

            if (response != null || response.ToString() != "")
            {
                Prf = response.ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool GetConfirmation(string response)
        {
            log.Info("Response: " + response.ToString());

            if (response != null || response.ToString() != "")
            {

                if (response.ToString().ToLower() == "yes" || response.ToString().ToLower() == "ye" || response.ToString().ToLower() == "y")
                {
                    return true;

                }
                else if (response.ToString().ToLower() == "nah" || response.ToString().ToLower() == "no" || response.ToString().ToLower() == "n")
                {
                    return false;
                }
            }
            return false;
        }

        [Command("prefix", RunMode = RunMode.Async)]
        [Summary("Changes the prefix if there's an input")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task Prefix()
        {
            log.Info("Prefix function booted up.");

            ObtainDatabaseInfo();

            string connectionString;
            connectionString = @"Data Source=" + datasource +";Initial Catalog=" + dbname + ";User ID=" + user + ";Password=" + pass + ";";

            log.Debug(connectionString);

            // Access the database
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                log.Info("Database connection established.");

                var guild = Context.Guild.Id;

                var selectQuery = "SELECT prefix FROM prefixes WHERE guildid = @Guild;";

                using (SqlCommand command = new SqlCommand(selectQuery))
                {
                    command.Parameters.AddWithValue("@Guild", guild);
                    command.Prepare();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Prf = reader.GetString(0);
                            log.Debug("Guild ID: " + guild + " / Prefix: " + Prf);
                        }
                    };
                };
            };

            await ReplyAsync("The current prefix for the bot is: ``" + Prf + "``. Enter a new prefix, otherwise leave blank to leave as is.");

            // Get the input of the user
            var response = await NextMessageAsync();
            AdminCommands ac = new AdminCommands();
            ac.GetInput(response.ToString());

            if (ReplyStatus == false)
            {
                await ReplyAsync("No input received, the prefix will stay as ``" + Prf + "``");
            }
            else if (ReplyStatus == true)
            {
                // Update database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    var guild = Context.Guild.Id;

                    var updateQuery = "INSERT INTO prefixes VALUES(@Guild,@Prefix) ON DUPLICATE KEY UPDATE prefix = @Prefix";

                    using (SqlCommand command = new SqlCommand(updateQuery))
                    {
                        // Two different ways to add
                        // command.Parameters.Add("@Prefix", SqlDbType.VarChar).Value = Prf;
                        command.Parameters.AddWithValue("@Prefix", Prf);
                        command.Parameters.AddWithValue("Guild", guild);
                        command.Prepare();
                        command.ExecuteNonQuery();
                    };
                };
            }

        }

        [Command("delete", RunMode = RunMode.Async)]
        [Summary("Deletes the messages (max 100) in the channel and places them into a log if a value is added")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task DeleteAndLog(params string[] args)
        {
            IEnumerable<IMessage> message = await Context.Channel.GetMessagesAsync().FlattenAsync(); ;
            int result;
            bool validReturn;
            bool withName = false;
            string replyMessage = "";
            string[] parameters = args;
            string filename = "";

            log.Info("Starting up the Delete and Log function.");

            if (parameters.Length == 0)
            {
                parameters = new string[] { "100" };
                var input = parameters[0];
                message = await Context.Channel.GetMessagesAsync(Convert.ToInt32(input)).FlattenAsync();
                replyMessage = "Are you sure you want to delete " + input + " lines? [Y/N]";
                validReturn = true;
            }
            else if (parameters.Length == 1)
            {
                if (int.TryParse(parameters[0], out result) && Convert.ToInt32(parameters[0]) != 0)
                {
                    var input = result;
                    if (input > 100)
                    {
                        // Max number
                        input = 100;
                    }

                    message = await Context.Channel.GetMessagesAsync(Convert.ToInt32(input)).FlattenAsync();
                    replyMessage = "Are you sure you want to delete " + input + " lines? [Y/N]";
                    validReturn = true;
                }
                else
                {
                    await ReplyAsync("Invalid input.");
                    validReturn = false;
                }
            }
            else if (parameters.Length == 2)
            {
                if (int.TryParse(parameters[0], out result) && Convert.ToInt32(parameters[0]) != 0)
                {
                    var input = result;
                    filename = parameters[1];
                    if (input > 100)
                    {
                        // Max number
                        input = 100;
                    }

                    message = await Context.Channel.GetMessagesAsync(Convert.ToInt32(input)).FlattenAsync();
                    replyMessage = "Are you sure you want to delete " + input + " lines? [Y/N]";
                    validReturn = true;
                    withName = true;
                }
                else
                {
                    await ReplyAsync("Invalid input.");
                    validReturn = false;
                }
            }
            else
            {
                replyMessage = "Are you sure you want to delete 100 lines? [Y/N]";
                validReturn = true;
            }

            if (validReturn == true)
            {
                // Get the confirmation of the user
                log.Info("Getting confirmation to delete");
                await ReplyAsync(replyMessage);
                var response = await NextMessageAsync();
                AdminCommands ac = new AdminCommands();
                log.Debug("Immediately after");

                if (ac.GetConfirmation(response.ToString()) == true)
                {
                    log.Info("Deleting and logging messages...");

                    FileInteraction fi = new FileInteraction();
                    List<Messages> msgList = new List<Messages>(); // Make a new list to pass

                    foreach (var item in message)
                    {
                        string lines;
                        if (item.Attachments.Count > 0 && withName == true)
                        {
                            lines = "[" + item.Timestamp + "] " + "(" + item.Channel + ") " + item.Author + ": " + item.Content + "\n<" + item.Attachments.Count + " item(s) attached>";
                            fi.Logged(lines, filename);
                        }
                        else if (item.Attachments.Count == 0 && withName == true)
                        {
                            lines = "[" + item.Timestamp + "] " + "(" + item.Channel + ") " + item.Author + ": " + item.Content;
                            fi.Logged(lines, filename);
                        }
                        else if (item.Attachments.Count > 0 && withName == false)
                        {
                            lines = "[" + item.Timestamp + "] " + "(" + item.Channel + ") " + item.Author + ": " + item.Content + "\n<" + item.Attachments.Count + " item(s) attached>";
                            fi.Logged(lines);
                        }
                        else
                        {
                            lines = "[" + item.Timestamp + "] " + "(" + item.Channel + ") " + item.Author + ": " + item.Content;
                            fi.Logged(lines);
                        }

                        if (item.Content.Contains("Deletion log") && item.Author.IsBot == true)
                        {
                            log.Debug("Ignoring bot message");
                        }
                        else
                        {
                            msgList.Add(new Messages(item.Author.ToString(), item.Timestamp.ToString(), item.Content)); // Add to the list

                            await item.DeleteAsync();
                            await Task.Delay(100);
                        }
                    }

                    if (withName == true)
                    {
                        string location = @"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\logs\" + fi.GetDate() + "-deletionlogs - " + filename + ".txt";

                        await Context.Channel.SendFileAsync(location, "Deletion log for " + fi.GetTimestamp() + ": " + filename);
                    }
                    else
                    {
                        string location = @"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\logs\" + fi.GetDate() + "-deletionlogs.txt";

                        await Context.Channel.SendFileAsync(location, "Deletion log for " + fi.GetTimestamp());
                    }
                    
                }
                else
                {
                    log.Info("No messages deleted and logged.");
                    await ReplyAsync("Will not delete messages.");
                }
            }
      
        }

    }
}
