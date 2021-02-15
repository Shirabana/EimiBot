using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace EimiBot2.Modules
{
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        private readonly Logger log = new Logger();
        private string datasource;
        private string dbname;
        private string user;
        private string pass;
        public string Prf { get; set; }
        public bool ReplyStatus { get; set; }

        private void obtainDatabaseInfo()
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

            file.Close();
            log.Info("Database info successfully retrieved.");
        }

        [Command("prefix")]
        [Summary("Changes the prefix if there's an input")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task Prefix(params string[] args)
        {
            log.Info("Prefix function booted up.");

            obtainDatabaseInfo();

            string connectionString;
            connectionString = @"Data Source=" + datasource +";Initial Catalog=" + dbname + ";User ID=" + user + ";Password=" + pass + ";";

            // Access the database
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

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
            InteractionClass ic = new InteractionClass();
            await ic.GetInput();

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
    }
}
