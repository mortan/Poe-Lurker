﻿//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Wohs Inc.">
//     Copyright © Wohs Inc.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Lurker.Models;
using Lurker.Services;
using Newtonsoft.Json.Linq;

namespace Lurker.Gem
{
    class Program
    {
        static void Main(string[] args)
        {
            var testGem = new Models.Gem()
            {
                Name = "Awakened Elemental Damage with Attacks Support",
            };
            testGem.SetUrl();

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://raw.githubusercontent.com/brather1ng/RePoE/master/RePoE/data/gems.json");
            var response = client.SendAsync(request).Result;

            var gemInfo = response.Content.ReadAsStringAsync().Result;

            var jobject = JObject.Parse(gemInfo);

            var gems = new List<Models.Gem>();
            foreach (var element in jobject.Children())
            {
                var children = element.Children();
                
                var requiredLevel = children["per_level"]["1"]["required_level"].FirstOrDefault();
                if (requiredLevel == null)
                {
                    continue;
                }

                var isSupport = (bool)children["is_support"].FirstOrDefault();
                var id = ((Newtonsoft.Json.Linq.JProperty)element).Name;

                var baseItem = children["base_item"].FirstOrDefault();
                if (baseItem is JValue)
                {
                    continue;
                }

                var displayName = (string)children["base_item"]["display_name"].FirstOrDefault();
                var gem = new Models.Gem()
                {
                    Id = id,
                    Name = displayName,
                    Level = (int)requiredLevel
                };

                gem.SetUrl();

                gems.Add(gem);
            }

            var lurker = new PathOfBuildingService();
            var build = lurker.Decode("eNrtfVtz28aW7vPmr0C5as-LZbvvF489U7IkS0okR1uSo2ReUg10Q0IMAgoAWlZOnf8-qwFSBCiABGVlZs6ZHcc0CazVl3X9VqObfPfv36Zp8NUVZZJn71_g1-hF4LIot0l2_f7F58uPr9SLf_-3ybszU938FH-YJam_82-Tv72r3wep--pS4EPAV5ni2lU_L9qiv8G10GQ2qT7lxdQA2ac8c4tr3U-nroiS1JXl4nKUmrL8ZKbu_YuLG2PzuxeBKSOX2b3WDRPmlZsVL4KpSbKLPPriqsMin93CgF4EXxN3d5pboLs8PzhYdLRXzNxiJDCNv707S829Ky4qUwUlvLx_sQvSMNfuKKmgDZPOoAEqlaavEcwSkxdv1rLtmym8bsV5ceucfeAgr6niEjNSv9JnZTpNMndi7kGDl8l0OUj0mjLMKUKcCcYUHmI_K9xBHLuoSr66vQJkeWOyaNkM0a_FEGsPOUevGVVsHcfpLK2S2zRxRWumkg-xHD3qw9vlAPFlXpl0_-zigVYjhvBrpTkFCQ7LsOHLl8Yx2MPxdZZUrt0Flq8VJoPjP8uTMs_aDBzr15xgTZVSG7i6docR1a8RyFdKOaiUq6S6abgHjFeB_GDESirJ-aD9nprM7OXlUiJYD1pttdQkpoMyPnd_dCjJIOW--9ZqEK9psEs5rLSsahm0XtNgmxIPq-ckidtq0WiNR0dvPfVxFm02Lk_4OStc6YqvrUAg6DiGM4i3rjUB_lpKrbDgmEqB-bpGzt21y8YN8MS56OYQYjN0146naJ0ptRxSrZWVJ27LSq1rtUdWrwhH41hWpfWKyNecUvAxwSSXcl0rXXFR-not9dYSO8hccX1_cZO4dDkzhhlfK7o2V1uEGMsxHY00gTbL9hP7ajyOWFo2Fuun1NCPcp5TlzoHDNat5Itn5Dgr8t99qky3Y9stpvmsFfqUXDvrhnzUpM9u7sskgtxVB_lzZ2cwupaAB4PnQ84_zb-6KfhBjT0AWrWS8nD8-5ACLltFCuuwUJr2sQwLrKpM9GU_t9ejZVx3shXHx6QAiZVJK8kNw5C93HvISOKT5PqmygCRrXIMQ6kbk5cr1ExuHvlPkOP3zG0rSGyewCMeNXIeW3S2nM4q07Ap53dAeePrjXIZWsdQn5pvmxk-Fi778350-x3yUR0cZHZWeMtb7QON5Xjczbs3dUXm3x1Pb_Oiqi_umTQq6yaPs9tZFWR14TRNyui3cBbHvj56AV0Udbl38PHjwd7l8c8H81G0WcovSZr-ls2moQfjzb_LmHHh6lgSRHmamtvS2fcvsiR9ESTw5sJzXkD8jKoR1JAb5ih0M61H4SP6ryulzXS-4Bg3wHnUGDn1y_tb5zVZjmD4kI4ba4PaRxA2NcgYWbrI3I-ge8jjI2hPwcya5DFm7hDIiyScVW4McY2qR4zA48nNZB0QNWKkTY4e0e4cw2ymnMfzEXqqnePSfHFjNbDvYgfmWs6jxCIivKuNswysiw2U2Ydu-o-ZSZPqfs6-vH7SrPHUV8ub_O5iduvjC9zxpl2CPE5O4E5zqfxwDwXs-xdVMZsvsNT91Cs0uzWKqD_XowAg4FebTJj6QdccQZnmHjTk9j5YQKEVVs_lG_7bO-huzn6Y5qFJyaKRPxYzgcJxsURF8Ivg2k2PvVRcZaypzJvjCgb8xo_6Td00vPvZmHS3ADhVBzxPPb8CTg9G3-kPL_pbmYOPmB6dNbyBb-7NXzDiBzU0OcH61Z2yNfA5QffuduOf8wYN8_hJoO0ncQQAAhKXiSrf2fz649mcu2leOU9Ctp2M7-FV00U9oafNB42cz2VhbnezWuyLZZXVufSQbDcj30BgskY_QTtv_lWz8h3FTa25OpvWre1msWR8ytD5yJHvWuvsA0ptZHWWznocZphyu3nt3vkoDd5TNxg8tNhR1Lsm8myMlHggUh65dOqqniCJRwfJpSzxWDPYM4DWs3Yuehx2HpNsGXuggeDuxmVzeQWtpLfltPio0H9ROZfCNNqT-ccsib4czkyxtVUvWxs9Yr591ITKv3AGkv4-lAdNOb-qiB6S7Wby0EDw0MLoKenRIWahhGn-pQ7v7Yksr22pA8_YivXf62zn3nvxdzlbO7-Pk8iRK0xqf4qPo7ZMOle3zIM1a5DHwXH0TIKBrr96dPE9gkFPFcxuedMjmPrqUwXjmZ8QaEa77Yc0z-2puU6ix_7avrfd-GvOoGF9gofSUVI_K1yUlN1IsxtFEBmiewAzvooud-HTtoNftvsUFDAO3J-BeE3aGvjiwpZDbbiexXOunLmF4g0_l-_gcVrcNx2nqdNcvSK6rSzqhv7S6gbq2zyzV0lme6Fa4hOSSffyPLX53dbJrWk9qJt_CrQYiz6LyPiqpuirAzo3t8SYNWvQ8D5fjiNPMcd6WpBsQR8rHW0fKhdR_GwG8eS-J7x3brQ6bU1s-7Df0yiI-FtSHsDw3QVYWOKKxQhqPP88Mv-Q59X3ZU-1Nc6qn4HUwLqJ1kujXL2znTT_wxmYU3H_l9ShK5n_8maWWb8e_cg8Hu481RQWDQzPIjZp-TQY0FpqOvdG5PJZ6Z_TrCw6rdxbmcm8-_VLTw9NBHUbb57Tg8UoB969zrM-_21ffy73fdzmSn8Xtwno9Cw11cMq-_Okb_JMBfhI-7-CMqydvueft5Nbw_QU-8ZstH0fAixctev5tafYc8P65i8TrO9kPykjv-ksWx13-8ZTBt_iH7C9Mp8VkYOKG0b2lr09n0Fq_5iU1Q68Zteu8Bskg4sk_RIMFFxza62X6lMT-plvW7F53kH51g33w6AHUR1k0Y3JKr9R4Kf4Yw4T-ik7Siq2rX2uCmn-rn52cVk4F5hm2DUxnj9rgA_tfaJ-PAA5s9z65xSMC6R2KJVS7mjE-Y5kjO0QpbjeIUgruUOwVmyHCrlDFSZ6hxEp0A7DgvIdruEFLmh41QIBN7zuUCwR3SFEYLXDOBM7mGF_AdXvlWeEpqSARoigO5LDTcqVZjtY-5tYKeGbompHMMXIjkCM-S6RwjucU0p2mJIShi0UVzuYSg6tQxd0R1Cp9Q4lEsMwEMUarjDC4RVDwwJTgna48qOBv9CYIDB0aFLBZU6Y3OEUJLLDCUdkh1ABUoBuCVzHTJAdjkAgGHOOQUS-V6Y1gp6EgNFgLLHw4pIMxg3jqWlAdFwiP2-syI6q20II5IgZIhiGqmE6hCsYDUMMwXWhYbKUKsF2GBMKhso58_TCkzM_e0FAXUJq0A7HXjOgEy8CirBXB2gQ1OEHSSn1AyC-FVy_l0KCZVZgLK1dyX6RM1oYh5jb-ufzk_rN326q6rZ8--bN3d3d61sIkHnsviWpex3l0ze3wAQW96o29Ve-2Te78N-H3V_3d0-O8-vfDn7_5Rv6HJ1U56_wL9e7V68-KxMd53-c_nIiouIu-hwfHn395epjfn73e_TL54Mffz7-8eDr0edXp5--3P95ER7Eu1_xj_JDcu3s9Zev-R9nr36_4p-TjH-ivx3a6W_0LjaxZL8n-u4fR-RV_OvPn-Iz9Gn_08f0W5EfnP3w4eYsPjnXdzCI8nf0a_Xt4y9Xr6of7j7nlTi_-zW-Up-qT-XV7u3hZZbZdO_k-Pz47AR9rH4Rd9XpZ_P71Q9n8W_V5ScTsrv4U_hDnl2eHJjLH-7ev68F9WYhqXfNfuuyEdv8U-1pXq5YEAyYFFBzHXPn-xp66MDA0Ag6cC_KxtAxsK4lHRFDhIRoPaZBpsD0RtARinGrXzpEB_bekgvBQ3TgZqMmTLTEZElHBxuE8INbklns0e1pkWk8omPqI1FrJsOEWLRVMqw6QsdYAsQjPYYOI07JGAVDIBrTHhGS8DHtaaTGtMcgZulRcoZ4PkYukGDkmPYw0qOGp9Qo8Qmqx2jXJ8uWmQ5PQ3PVEvNiP2rPPHwaaNkfGVScz-ktQjZoMT6XtwgHu8aKUdEi5MM2A0l-jG0pJvmoKAhgZQwdkpSPsi2t9Kh-2ShbxZLgccGyo5I1_QoxRn4MENooo_YYrGVcg9mBYzCaMQJkbFRsI4BT2mF_MPhiAFl0VJQRalz6kmJUHiZYjIkykBzkqOiGoeNRliC5GGUJFI2yaKFHRUuhcWsaWA2ma8CdLb0t6KAWWaIf-ABlR12h-LrEv_mUV66-5y8uPtRVy8-JuwtKZ4ro5qIqfHn2Z55Pf62LKb-56siZ6tTczkstf-_k4elMvfmqMtV-EseuqDd1LcomT_jLYr7v6sp2Xh_59xeuqmu0Wenmq-n1skV9uSldm_2eQFpvG5svXZwbvxz6Nvj86fgfnw8mFzfma5FnmfsXYC7_tQyuCnN7C4VpOfkpivxRrbJa3PrZlXUFOPmcJX_MXHC8_zYgDjIZi6SJuXVhhC2EcC65MyTmKDShoIoZ8HhqtCVGIhdjwLohdjTkWvJ4Uo-vFsfbQKHJfL8aNIwmc3W8DT688n8O_eukJj13f7wNBJkcT6H0jhJPgycY_T1IHh5r1yue810GEyg92vearYFBszew5otNWbnC78stKr_q1KEIzl1U75mavKTy70GVt7Z9NHv8_NbRybmL_X7YMsCeBAqwR7tD_PX60EHQbHCH-mVSb5Ve3Le5K8FMqyC893XJykC9VXpxdfVKuno93z0_mHwo_EPDM2e-TI5mYZGUwV5SRKnrqC40zkTWkBDKVRlzAOORQyqMpMTMIReFIcZOCxcZyqLQMYVdbLQwWkP4iXHUVR1vqY4vVXf-yv85bKlNrqjt_0SFiStn_-_ywX5wXZgMBGkengQFtKPcxaGFoN4HPHnJqPayXTyyDM5NBVIHdSl_eWq-JdPZdEWaLylp3_S7SqGhNfp9GKhkw8bUryPa63uXNy44g0vmBtRSQoDyj2naGnLSyRDSb-gLYMFMDLU2j0H-klnFDbNGaCMVxDMjrDWRi6WxsQCc4CJCoq5ziZZWrlr64F11HBlvwA3l5GUtoprWe0VzFebs19cmvC2FxqDrfWv1Hp-5YjBCtdHPhex3-Aa34Gjn3kPnfazQeEXUNIcQWbMhqq6DevIPKUhtTo07Y6ulANaZVSD9mvbqBqL_grhfZazHrR7W7ib9S3dt3Rkuokjj2Mk40ppowYlDSBIrKeY41IRJxQUSODJOREZzEmFOwa0kDq1g8aB3UdT1rsNX54PeRZZGe1kk1zDkYC-fTkFLtfeASuu1vCDPgqMExCYGI-hLWot_332DOAnj8K4y7FtaDXvIS6xrH_MHUtruhWWn82qW1dZUnxvyERgEXNwv5_OSoLoZ_wykMbtue-WAXnmfXn3shfQJPYD0whn0fwVNdlzRYhbHIQXvwwhDlSIZMiy2IQ-pRsQww0kcI0EiRrRW0hAaae44fDSAi1lXnaw_zx36HLcmVFK5JsO1b3VcsOPCBlgaxmZ1uvZkv_U_AK3A_UcpC8LBwbeqMEE7US3VsGeyACCEC2a3vh8a7DU3IETbJE58hnugZcPm9UAjOp7rN574421gDkUCyaE5pBHEedGwtxqv58DqOXzKs1edtApzcI_n0G8f4mmhWtEYSawdCZWQIQKvlobT0EFutYILijDhXEWK8zjCUns3h4itIhZysBEXdu1DPHOoFv9fh2rZq7JDiAd398V08mkWzmC-9fP4tsogrSojBcc8Jv4oPEKYYhQyjp0KJaTSELRmNTUiYpGCmhATHWLDhOXUICO6KsNroWtLiZQN4B-8BuIESRzc57Pa1bIGjwNI9EKHuO2jI1Cm9xPRbcKrpz7W65otnh4XQajQbaIHYfutg3WJEeezrMZUy_jaDtPrgFi_flSvfs4Alk6DehtuYrLJLhRPMMIfa203maJdZqBIsojA_zJkceRCqB5ibFWIYmElCMJFGLSpARVhYaEisVQKuGqYQnGkSEdXkg3pqht-hepq6iUmtUh8DD1I63n7x_GtlDPkhH7PReOJHb9sImdrh1_fbQIOUviDvUuN-gPstUKhQ7KSlj352jH260j3pMWPYC3BkSkygDmTveK-9E19cGmniuA6jKiLBQslpURIQ6MQ0GroqIWYhyjVgH-ctlggFGuDlFZCGBpzBS86lquJsZX99Ir4tViD5hn1NyFPuOy6uvFyWEPMO3WBD3WrDE05QHEvVPluEILRgEuAan2CA9uBVHU_S83kp-z-W7A7na0Ub0xJ5yixKLSRU9hGHEptpJFfwYcKO8JOaIsEh-xjMNR0SCuKhDLKYKwR7uYbMPSl2MEj-hHkbgpQDEyiDD5X07z0520AK4BXiIXBLU_7Tbr1WjPPRyFGNv7UZOWWfAFU1EWwq2FgBoHFyxm4O8lpL88AsHlngTbGVGG4fwlkt_ozKZLFIsdZkU-T0k12p666AZMPPqam_NLJG5iHPDY0DGOQP1TK1nJmLOEWI4KsYqFiEVzSMeQVjSngREgkgBF1hCzRXWQPEK8di5ZqwJ3ggybk73OrWMKa-tgmTB9gQC0QO_NHfpsBB64-o9lAO59j_U64-ZcFrIV2i1Y6_LTmX8aTLRoYUMXKqsXp7uHx3mQ3hflMWytO9RbWMklBz_Np-R1BFnzc-C0XHQAmQfzKEELgX8QY50wJE2GiuGbcGkxigGcAxRFCoBQtSWiclADJNBaU8o5W2KBWxIpW9Jqc3acP0oE784uULkP84jjGkOBon-AOvgFUSnzXrljIbj8xU78L9kFufhWyI7HIgKBkZJnHo4ThGGkAOC4yRMsojplSiJJQYysAKjnrsJGShzGVNBYy7kpMsAGJEdmV2PF0Ost8MPBhE2Djn_PAeeOrkT6J-cOBUFs_Jga0MANHpZ1ac5P42FjxXaxY3ZXHKSuY3yrjfT0KHeHGglSstZJZx0PHBZgaFPcaokVkERUE0KSSxBnuMEQBSTQbZ3KE9AiwXkrcmxUlCGad0OYkC1HxbUTFe6PlfvI1yRo0uQ-eCorwmPLEL48GR_dhkdjHEVPqUIVQIBvAB0wRojHBUC9xZyIZQe0sqbZaxMppiJVEE8hZhEHSUhpgOXVdQQk0ICiKuoLiemMK6pUdViPw8Qjnrktp6Ncms34lveQNeujFaV2Og7XBtL9mnafEj5DQAv-tYEk22fO7tKrgB3fn0k5K0zL261EoNtjEIQI4AVWqscLFljlwfwaRk0AwsJG0zEYYwIUBQM40QzFWtqsgOmkm_TaozWJllb4XzUKKp23Z9eO2xaG1so4CD992UzZLtpABk2whbwOA7LIwWRnnxbSBCOb2Nr1_jBX28qKY3VaD1QvuLy-vTAXJ6iFcHNy7pqIB14geC1iF3HDFoU6jVhoBwFijiBkN2SlUBAlqNbyByEpCEXLAzP6RnDNIsAjHmLvV1cCuoW-WXC9JjXt779Tod4nCPkCy9add7yDuQgSubRE-hvfBwwGflTp-JRM-Zqw3fQ5JXPUUIxe3ANKgvRuIw5NTB0qeBnvprH5s80jcEeIYSSZEjKSKrV9RDaUWIRGht-cYh1hGSvl9cBZzrEODIdr47_3SUQjhpytuOgSIrS0DFpw1O8iaBbWyc9svnUw9Mu7SNM833gbNfmb_dQ3g-93Ceo6lGsH5yt8bdFuAJmh2QU_w_Chyp4sgKef3i_thirNZAdbr4lnqy7ywXtMepjaNnNevxeC-QvLUuLSsfBw6n62JQsqGDICyVdr7QUQo43FkFUbCQI4gmpqIKTAbSpkNKYsIp46EDFOHCaTcbpqQuOsk3QXS1tl6v15Vh-FmD-ujNcfl91UCZVNcD5O0lyZfYrmJ-g7CcmvJdW5BA8_5UI9kzyAQQqrPvDfU4WejazjFWCgAyDmJJUYYhwpsP2QC2RhwMfFOQSHYR4AMPXi2EIyUNtJyFLJIm9WVlDWuwf_HusZBZsNig7n_Bc5BcO-zWjMNYSYXaeKfP2zSnwl94LKcCw6li5ZCUPAXLglVyDgD9QylkQmxojbyj2pDKR3omGuuEXwSW-jvn6Gtq72-J-0wmTkaPkyT6RQ6aaDwoP4U1QZ0Bjnd6SiKYsdiAFEO_hBpHEQzZR1loEAnImZJHGGmAYaFPNbCEku7-msv3Ai0TpnqO5Tp1z1bD2X8GtfisdOQRD-kJmtWMi-y_G6YDmqIvEgAKdSPnJLyNjVrtHoB4K4EnN13v8F8bT0ORlK6xg2hSi6qwRRlMYBhowAjkNhxDgqUljiHIhdjaoTkgsYhiQhgOqioBfG77iPsd8NEsQK1ry67jYPGK9iqnbtYawVt7lj-QWngvwu6nbq6Lth5DvYS45H5bxNUJn0PzQ_T-9sbaMxsdA2qoQBEPBTgGDgWIWBkY2OELEYGaesiK2Pu96vEVsgQUFsM9SH4D5eaGW7k_2rXWBYfjQV_r4_0PSf_GWp_69cUknzSCGVQl0yAQhhgDBIh5rhCOISahsUEcy0ExD4oNi1yHCAfQwArOAMvMRgpGnKupFjjKHRFfeQ71Cc2l05zrzpzmauKevkb3KV3g9Ca7kxa5os-X9L-Ve-R7ORxLl0XML_msyKobsCP83rv3kYfFj2K37uBcFKAbYFMMrcRo0hQJolsJKVRxgKkZ1bS0EIUFFAFI8DskhClkUBICCs0FsaGkkVxCHbAycrTOvFPjDIeo8i-0jmHHvZm1Ua9EUWxCzGGVCWVxpE_-yYJAze2iCnu9ztLGoNeKTi3R5mQ8fwDWEM5xow8erLx_2Rt8N-jt74lj7PUXINm6pp5Q7iNwb0QYEao3WQcIgCMiCMRq9BGsUEQT3UMUVc7CmWDolEsrQ0j42IAo5pEHA0_DuTsvzH2dvY7NWugiw0-taZ9AN5NUh_YytHhl31X-H1JhneIPmcY7ltH2S_MNVQaTaG_wSIUWL0QylkjGJU6YiKmNPRnUv123yg0mFJL_a4kE1urOZQhyDB45VprFJn_oRYxiJFHqo_2ZM_Redsr_jirXJom1-6ZFU77lnd-LPzX49WLyxuws1AGXNthimNkBY8E5RbH2AhIudJgZwmLIgrB3QIU4ypyhgHCpkZpAgCaP0q5__XYeXXn4XMBY_8F5OUNBN-_uqykfas7R_e3We5R-bjVOfBHizi23BIaRUoQn4hNxJn2KjOgPAXKAxdFoZUiFBjU6oyVJLSYupUHMZL-c3Vucw6-SPPF96Evvj9hcTRo8TtGNUlz8qb9lQbvXzQP59qnzzosDZHfMbWk0OPabJ0De_iRqQ0srSNSGD8eRrOHqEWDeobafGfRgkSO67h1pPThV2ZajS6_TmVBpR4TLb7y9OFUVt_gWt8f_dDhY7rFl10tSMS4WbQPqNKBVlsipoMTbREtpDE_r7XmrFb7iNYIk-wV8MWduQ12w_uyNAtTD3qO-67a5miWrTxg9Ajx-hE2hvEsTBtksVDfln31uveohseNh2yrUu_Dz6DTngjUY5mjhbVV3Bry-y1FMRTyRsS8UZrAT9AE3t4hnyDTnpDca5JPCBnrWPE4i36UCcakgn6N-FFs0sJw2uh3081tDqSYEf5BRg_2GaygJ6WNyWkjk9qbeVarD0nXW6PqH6fJszi5fvRTM5D5Vn6BJ8zz1JlsPu7HP07j02R9-hMKwOl9veXP_87bJjbnqY_LD3lZLn8E5-Jo9-zgfF0nJ_ld86N-m9oPAQHb-gjNjbOXeT24zVww_e7vG203--YXX-y2bPUvz4AAtuDzT38Wx0n62N69WSj43ZvV34_9TzeGRTQ=");
        }
    }
}
