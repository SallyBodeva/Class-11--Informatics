﻿using InfluencerManagerApp.Core.Contracts;
using InfluencerManagerApp.Models;
using InfluencerManagerApp.Models.Contracts;
using InfluencerManagerApp.Repositories;
using InfluencerManagerApp.Repositories.Contracts;
using InfluencerManagerApp.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfluencerManagerApp.Core
{
    public class Controller : IController
    {
        private IRepository<IInfluencer> influencers;
        private IRepository<ICampaign> campaigns;
        public Controller()
        {
            influencers = new InfluencerRepository();
            campaigns = new CampaignRepository();
        }

        public string ApplicationReport()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var influencer in influencers.Models.OrderByDescending(i => i.Income).ThenByDescending(i => i.Followers))
            {
                sb.AppendLine(influencer.ToString());

                if (influencer.Participations.Any())
                {
                    sb.AppendLine("Active Campaigns:");

                    foreach (var campaign in campaigns.Models.Where(c => c.Contributors.Contains(influencer.Username)).OrderBy(c => c.Brand))
                    {
                        sb.AppendLine($"--{campaign.ToString()}");
                    }
                }
            }

            return sb.ToString().TrimEnd();
        }

        public string AttractInfluencer(string brand, string username)
        {
            if (influencers.FindByName(username) == null)
            {
                return string.Format(OutputMessages.InfluencerNotFound, nameof(InfluencerRepository), username);
            }

            if (campaigns.FindByName(brand) == null)
            {
                return string.Format(OutputMessages.CampaignNotFound, brand);
            }

            IInfluencer influencer = influencers.FindByName(username);
            ICampaign campaign = campaigns.FindByName(brand);

            if (campaign.Contributors.Contains(influencer.Username))
            {
                return string.Format(OutputMessages.InfluencerAlreadyEngaged, username, brand);
            }

            bool isEligible = true;
            if (campaign.GetType().Name == nameof(ProductCampaign) && influencer.GetType().Name == nameof(BloggerInfluencer))
            {
                isEligible = false;
            }
            if (campaign.GetType().Name == nameof(ServiceCampaign) && influencer.GetType().Name == nameof(FashionInfluencer))
            {
                isEligible = false;
            }

            if (!isEligible)
            {
                return string.Format(OutputMessages.InfluencerNotEligibleForCampaign, username, brand);
            }

            double profit = influencer.CalculateCampaignPrice();

            if (campaign.Budget < profit)
            {
                return string.Format(OutputMessages.UnsufficientBudget, brand, username);
            }

            influencer.EarnFee(profit);
            influencer.EnrollCampaign(brand);
            campaign.Engage(influencer);

            return string.Format(OutputMessages.InfluencerAttractedSuccessfully, username, brand);
        }

        public string BeginCampaign(string typeName, string brand)
        {
            if (typeName != nameof(ProductCampaign) && typeName != nameof(ServiceCampaign))
            {
                return string.Format(OutputMessages.CampaignTypeIsNotValid, typeName);
            }
            if (this.campaigns.FindByName(brand)!=null)
            {
                return string.Format(OutputMessages.CampaignDuplicated, brand);
            }
            ICampaign campaign = null;
            switch (typeName)
            {
                case nameof(ProductCampaign):
                    campaign = new ProductCampaign(brand);
                    break;
                case nameof(ServiceCampaign):
                    campaign = new ServiceCampaign(brand);
                    break;
                default:
                    return string.Format(OutputMessages.CampaignTypeIsNotValid, typeName);
            }
            this.campaigns.AddModel(campaign);
            return string.Format(OutputMessages.CampaignStartedSuccessfully, brand,typeName);
        }

        public string CloseCampaign(string brand)
        {
            ICampaign campaign = campaigns.FindByName(brand);

            if (campaign == null)
            {
                return string.Format(OutputMessages.InvalidCampaignToClose);
            }

            if (campaign.Budget <= 10000)
            {
                return string.Format(OutputMessages.CampaignCannotBeClosed, brand);
            }

            foreach (var influencer in campaign.Contributors)
            {
                var currInfluencer = influencers.FindByName(influencer);
                currInfluencer.EarnFee(2000);
                currInfluencer.EndParticipation(campaign.Brand);
            }

            campaigns.RemoveModel(campaign);
            return string.Format(OutputMessages.CampaignClosedSuccessfully, brand);
        }

        public string ConcludeAppContract(string username)
        {
            IInfluencer influencer = influencers.FindByName(username);

            if (influencer == null)
            {
                return string.Format(OutputMessages.InfluencerNotSigned, username);
            }

            if (influencer.Participations.Any())
            {
                return string.Format(OutputMessages.InfluencerHasActiveParticipations, username);
            }

            influencers.RemoveModel(influencer);
            return string.Format(OutputMessages.ContractConcludedSuccessfully, username);
        }

        public string FundCampaign(string brand, double amount)
        {
            ICampaign campaign = campaigns.FindByName(brand);

            if (campaign == null)
            {
                return string.Format(OutputMessages.InvalidCampaignToFund);
            }

            if (amount <= 0)
            {
                return string.Format(OutputMessages.NotPositiveFundingAmount);
            }

            campaign.Gain(amount);
            return string.Format(OutputMessages.CampaignFundedSuccessfully, brand, amount);
        }

        public string RegisterInfluencer(string typeName, string username, int followers)
        {
            if (typeName!=nameof(BloggerInfluencer) && typeName!=nameof(BusinessInfluencer) && typeName!=nameof(FashionInfluencer))
            {
                return string.Format(OutputMessages.InfluencerInvalidType, typeName);
            }
            if (this.influencers.FindByName(username)!=null)
            {
                return string.Format(OutputMessages.UsernameIsRegistered, username);
            }
            IInfluencer influencer = null;
            switch (typeName)
            {
                case nameof(BusinessInfluencer):
                    influencer = new BusinessInfluencer(username,followers);
                    break;
                case nameof(BloggerInfluencer):
                    influencer = new BloggerInfluencer(username, followers);
                    break;
                case nameof(FashionInfluencer):
                    influencer = new FashionInfluencer(username, followers);
                    break;
                default:
                    return string.Format(OutputMessages.InfluencerInvalidType, typeName);
                    break;
            }
            this.influencers.AddModel(influencer);
            return string.Format(OutputMessages.InfluencerRegisteredSuccessfully,username);
        }
    }
}
