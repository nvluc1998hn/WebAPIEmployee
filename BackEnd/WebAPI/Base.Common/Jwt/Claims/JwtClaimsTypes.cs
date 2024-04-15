using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Jwt.Claims
{
    public static class JwtClaimsTypes
    {
        public static string Sub = "sub";
        public static string UniqueName = "unique_name";
        public static string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        public static string Nickname = "nickname";
        public static string Iat = "iat";
        public static string Email = "email";
        public static string Gender = "gender";
        public static string Address = "address";
        public static string Audience = "aud";
        public static string Issuer = "iss";
        public static string NotBefore = "nbf";
        public static string Expiration = "exp";
        public static string UpdatedAt = "updated_at";
        public static string IssuedAt = "iat";
        public static string Nonce = "nonce";
        public static string Jti = "jti";
        public static string Events = "events";
        public static string ClientId = "client_id";
        public static string Scope = "scope";
        public static string Id = "id";
        public static string IdentityProvider = "idp";
        public static string Role = "role";
        public static string ReferenceTokenId = "reference_token_id";
        public static string Confirmation = "cnf";
        public static string AccessToken = "access_token";
        public static string ClientIP = "cip";
        public static string Version = "ver";
        public static string UserName = "UserName";
        public static string FullName = "FullName";
        public static string AvatarUrl = "AvatarUrl";
        public static string Permission = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        public static string UserType = "UserType";
        public static string CompanyType = "CompanyType";
        public static string CompanyId = "CompanyId";
        public static string XnCode = "XnCode";
        public static string CustomerCode = "CustomerCode";
        public static string LoginUserId = "LoginUserId";
        public static string LoginUserName = "LoginUserName";
        public static string LoginUserType = "LoginUserType";
        public static string LoginCompanyType = "LoginCompanyType";
        public static string LoginCompanyId = "LoginCompanyId";
        public static string LoginPermissions = "LoginPermissions";
        public static string SessionKey = "SessionKey";
    }
}
