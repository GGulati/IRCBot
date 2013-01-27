using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCBot.Plugins
{
    public partial class Administrator : Plugin
    {
        public const string DEFAULT_USER_GROUP_NAME = "default";

        Dictionary<string, AdminstratorPlugin.Command[]> accountGroups;
        Dictionary<string, string> accounts;

        public Administrator()
        {
            accountGroups = new Dictionary<string, AdminstratorPlugin.Command[]>();
            accounts = new Dictionary<string, string>();

            AdminstratorPlugin.Command.Initialize(this);
        }

        public void AddAccountGroup(string accountGroup, params AdminstratorPlugin.Command[] commands)
        {
            string group = accountGroup.ToLower();
            if (accountGroups.ContainsKey(group))
            {
                accountGroups[group] = commands;
            }
            else
                accountGroups.Add(group, commands);
        }

        public void RemoveAccountGroup(string accountGroup)
        {
            string group = accountGroup.ToLower();

            if (!accountGroups.ContainsKey(group))
                return;

            for (int i = 0; i < accounts.Count; i++)
            {
                if (accounts.ElementAt(i).Value == group)
                {
                    accounts.Remove(accounts.ElementAt(i).Key);
                    i--;
                }
            }

            accountGroups.Remove(accountGroup);
        }

        public void AddAccount(string accountName, string accountGroup)
        {
            string account = accountName.ToLower();
            string group = accountGroup.ToLower();

            if (!accountGroups.ContainsKey(group))
                return;
            if (accounts.ContainsKey(account))
                accounts[account] = group;
            else
                accounts.Add(account, group);
        }

        public void RemoveAccount(string accountName)
        {
            string account = accountName.ToLower();

            if (accounts.ContainsKey(account))
                accounts.Remove(account);
        }

        public string GetAccountGroup(string accountName)
        {
            string account = accountName.ToLower();
            if (accounts.ContainsKey(account))
                return accounts[account];
            return null;
        }
		
		public string[] GetAccountMembers(string accountGroup)
		{
			string group = accountGroup.ToLower();
			
			List<string> members = new List<string>();
			
			foreach (var pair in accounts)
			{
				if (pair.Value == group)
					members.Add(pair.Key);
			}
			
			return members.ToArray();
		}

        public AdminstratorPlugin.Command[] GetCommandsAtGroup(string accountGroup)
        {
            string group = accountGroup.ToLower();

            if (accountGroups.ContainsKey(accountGroup))
                return accountGroups[group];
            return null;
        }
		
		public string[] GetAccountGroups()
		{
			return accountGroups.Keys.ToArray();
		}
    }
}
