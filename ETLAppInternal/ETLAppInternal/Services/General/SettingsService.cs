﻿using ETLAppInternal.Contracts.Services.General;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ETLAppInternal.Services.General
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettings _settings;
        private const string UserName = "UserName";
        private const string UserId = "UserId";
        private const string IsFirstRun = "IsFirstRun";

        public SettingsService()
        {
            _settings = CrossSettings.Current;
        }

        public void AddItem(string key, string value)
        {
            _settings.AddOrUpdateValue(key, value);
        }

        public string GetItem(string key)
        {
            return _settings.GetValueOrDefault(key, string.Empty);
        }

        public string UserNameSetting
        {
            get => GetItem(UserName);
            set => AddItem(UserName, value);
        }

        public string UserIdSetting
        {
            get => GetItem(UserId);
            set => AddItem(UserId, value);
        }

        public string IsFirstRunSetting
        {
            get => GetItem(IsFirstRun);
            set => AddItem(IsFirstRun, value);
        }
    }
}
