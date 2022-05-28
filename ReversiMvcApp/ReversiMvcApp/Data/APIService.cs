using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ReversiMvcApp.Models;

namespace ReversiMvcApp.Data
{
    public class APIService
    {
        private readonly HttpClient httpClient;

        public APIService()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5003/");
        }

        public Spel DoeZet(string token, string spelertoken, int kolom, int rij)
        {
            Spel resObject = null;

            HttpResponseMessage res = httpClient.PutAsync(
                $"/api/spel/{token}/zet?token={spelertoken}&rij={rij}&kolom={kolom}", new StringContent("")
            ).Result;
            if (res.IsSuccessStatusCode)
            {
                resObject = res.Content.ReadAsAsync<Spel>().Result;
            }

            return resObject;

        }

        public bool CheckIfAfgelopen(string speltoken)
        {
            bool resObjecten = new();

            var res = httpClient.GetAsync($"/api/Afgelopen/{speltoken}").Result;

            if (res.IsSuccessStatusCode)
            {
                resObjecten = res.Content.ReadAsAsync<bool>().Result;
            }

            return resObjecten;
        }


        public List<Spel> GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler()
        {
            List<Spel> resObjecten = new();

            var res = httpClient.GetAsync("/api/spel/").Result;

            if (res.IsSuccessStatusCode)
            {
                resObjecten = res.Content.ReadAsAsync<List<Spel>>().Result;
            }

            return resObjecten;
        }

        public List<Spel> GetSpellenDoorSpelerToken(string spelertoken)
        {
            List<Spel> resObjecten = new();

            var res = httpClient.GetAsync($"/api/SpelSpeler/{spelertoken}/").Result;

            if (res.IsSuccessStatusCode)
            {
                resObjecten = res.Content.ReadAsAsync<List<Spel>>().Result;
            }

            return resObjecten;
        }

        public Spel MaakSpel(string speler1Token, string omschrijving)
        {
            Spel resObject = null;

            var formUrlEncodedContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("spelerToken", speler1Token),
                new KeyValuePair<string, string>("omschrijving", omschrijving)
            });

            HttpResponseMessage res = httpClient.PostAsync("/api/spel/", formUrlEncodedContent).Result;

            if (res.IsSuccessStatusCode)
            {
                resObject = res.Content.ReadAsAsync<Spel>().Result;
            }

            return resObject;
        }

        public Spel GetSpel(string token)
        {
            Spel resObject = null;

            var res = httpClient.GetAsync($"/api/spel/{token}").Result;

            if (res.IsSuccessStatusCode) resObject = res.Content.ReadAsAsync<Spel>().Result;

            return resObject;
        }

        public List<Spel> GetAll()
        {
            List<Spel> resObject = new();
            // Ophalen uit API
            var res = httpClient.GetAsync($"/api/spellen").Result;
            if (res.IsSuccessStatusCode)
            {
                resObject = res.Content.ReadAsAsync<List<Spel>>().Result;
            }

            return resObject;
        }

        //TODO
        public bool Delete(string id, string spelerToken)
        {
            // Ophalen uit API
            var resultaat = httpClient.DeleteAsync($"/api/spel/{id}/?token={spelerToken}").Result;

            return resultaat.IsSuccessStatusCode;
        }

        public Spel Deelnemen(string id, string spelerToken)
        {
            Spel resObject = null;

            HttpResponseMessage res = httpClient.PutAsync(
                $"/api/spel/{id}/join/?token={spelerToken}", new StringContent("")
            ).Result;
            if (res.IsSuccessStatusCode)
            {
                resObject = res.Content.ReadAsAsync<Spel>().Result;
            }

            return resObject;
        }
    }
}
