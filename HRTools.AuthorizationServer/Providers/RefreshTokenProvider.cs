using System;
using System.Threading.Tasks;
using HRTools.AuthorizationServer.Entities;
using HRTools.AuthorizationServer.Services;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Master;
using Microsoft.Owin.Security.Infrastructure;

namespace HRTools.AuthorizationServer.Providers
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IAuthService _authService;

        public RefreshTokenProvider(IConfigurationManager configurationManager,
            IAuthService authService)
        {
            Guard.ConstructorArgumentIsNotNull(configurationManager, nameof(configurationManager));

            _configurationManager = configurationManager;
            _authService = authService;
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            //using (AuthService _repo = new AuthService(_configurationManager))
            //{
                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                var token = new RefreshToken()
                {
                    Id = Helper.GetHash(refreshTokenId),
                    ClientId = clientid,
                    Subject = context.Ticket.Identity.Name,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };

                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                token.ProtectedTicket = context.SerializeTicket();

                var result = await _authService.AddRefreshToken(token);

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }

            //}
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            if (allowedOrigin == null) allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = Helper.GetHash(context.Token);

            //using (AuthService _repo = new AuthService(_configurationManager))
            //{
                var refreshToken = await _authService.FindRefreshToken(hashedTokenId);

                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    var result = await _authService.RemoveRefreshToken(hashedTokenId);
                }
            //}
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}