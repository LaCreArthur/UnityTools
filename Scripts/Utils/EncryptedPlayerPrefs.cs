using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Toolbox.Utils
{
    public static class EncryptedPlayerPrefs
    {
        // Encrypted PlayerPrefs
        // Written by Sven Magnus
        // MD5 code by Matthew Wegner (from [url]http://www.unifycommunity.com/wiki/index.php?title=MD5[/url])

        // Modify this key in this file :
        private static string privateKey = "8HDcHjJlSTgoe6gzpvYe";

        // Add some values to this array before using EncryptedPlayerPrefs
        public static string[] keys = { "hey", "huy", "hoy" };


        public static string Md5(string strToEncrypt)
        {
            UTF8Encoding ue = new UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }

        public static void SaveEncryption(string key, string type, string value)
        {
            int keyIndex = (int)Mathf.Floor(Random.value * keys.Length);
            string secretKey = keys[keyIndex];
            string check = Md5(type + "_" + privateKey + "_" + secretKey + "_" + value);
            PlayerPrefs.SetString(key + "_encryption_check", check);
            PlayerPrefs.SetInt(key + "_used_key", keyIndex);
        }

        public static bool CheckEncryption(string key, string type, string value)
        {
            int keyIndex = PlayerPrefs.GetInt(key + "_used_key");
            string secretKey = keys[keyIndex];
            string check = Md5(type + "_" + privateKey + "_" + secretKey + "_" + value);
            if (!PlayerPrefs.HasKey(key + "_encryption_check")) return false;
            string storedCheck = PlayerPrefs.GetString(key + "_encryption_check");
            return storedCheck == check;
        }

        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            SaveEncryption(key, "int", value.ToString());
        }

        public static void SetBool(string key, bool value)
        {
            SetInt(key, Convert.ToInt32(value));
        }

        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
            SaveEncryption(key, "float", Mathf.Floor(value * 1000).ToString());
        }

        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            SaveEncryption(key, "string", value);
        }

        public static void SetDouble(string key, double value)
        {
            var storeBytes = BitConverter.GetBytes(value);
            var storeIntLow = BitConverter.ToInt32(storeBytes, 0);
            var storeIntHigh = BitConverter.ToInt32(storeBytes, 4);
            PlayerPrefs.SetInt($"{key}_low", storeIntLow);
            PlayerPrefs.SetInt($"{key}_high", storeIntHigh);

            SaveEncryption($"{key}_low", "int", storeIntLow.ToString());
            SaveEncryption($"{key}_high", "int", storeIntHigh.ToString());
        }

        public static int GetInt(string key) => GetInt(key, 0);

        public static float GetFloat(string key) => GetFloat(key, 0f);

        public static string GetString(string key) => GetString(key, "");

        public static int GetInt(string key, int defaultValue)
        {
            int value = PlayerPrefs.GetInt(key);
            if (!CheckEncryption(key, "int", value.ToString())) return defaultValue;
            return value;
        }

        public static bool GetBool(string key, bool defaultValue = false)
        {
            int tmp = GetInt(key, Convert.ToInt32(defaultValue));
            return Convert.ToBoolean(tmp);
        }

        public static float GetFloat(string key, float defaultValue)
        {
            float value = PlayerPrefs.GetFloat(key);
            if (!CheckEncryption(key, "float", Mathf.Floor(value * 1000).ToString())) return defaultValue;
            return value;
        }

        public static string GetString(string key, string defaultValue)
        {
            string value = PlayerPrefs.GetString(key);
            if (!CheckEncryption(key, "string", value)) return defaultValue;
            return value;
        }

        public static double GetDouble(string key, double defaultValue)
        {
            int value_low = PlayerPrefs.GetInt($"{key}_low");
            int value_high = PlayerPrefs.GetInt($"{key}_high");

            if (!CheckEncryption($"{key}_low", "int", value_low.ToString())) return defaultValue;
            if (!CheckEncryption($"{key}_high", "int", value_high.ToString())) return defaultValue;

            var retrieveBytes = new byte[8];
            Array.Copy(BitConverter.GetBytes(value_low), retrieveBytes, 4);
            Array.Copy(BitConverter.GetBytes(value_high), 0, retrieveBytes, 4, 4);
            return BitConverter.ToDouble(retrieveBytes, 0);
        }

        public static bool HasKey(string key) => PlayerPrefs.HasKey(key);

        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.DeleteKey(key + "_encryption_check");
            PlayerPrefs.DeleteKey(key + "_used_key");
        }
    }
}