/* 
 * DRES Client API
 *
 * Client API for DRES (Distributed Retrieval Evaluation Server), Version 2.0.0-RC4
 *
 * OpenAPI spec version: 2.0.0-RC4
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = IO.Swagger.Client.SwaggerDateConverter;
namespace IO.Swagger.Model
{
    /// <summary>
    /// ApiViewerInfo
    /// </summary>
    [DataContract]
        public partial class ApiViewerInfo :  IEquatable<ApiViewerInfo>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiViewerInfo" /> class.
        /// </summary>
        /// <param name="viewersId">viewersId (required).</param>
        /// <param name="username">username (required).</param>
        /// <param name="host">host (required).</param>
        /// <param name="ready">ready (required).</param>
        public ApiViewerInfo(string viewersId = default(string), string username = default(string), string host = default(string), bool? ready = default(bool?))
        {
            // to ensure "viewersId" is required (not null)
            if (viewersId == null)
            {
                throw new InvalidDataException("viewersId is a required property for ApiViewerInfo and cannot be null");
            }
            else
            {
                this.ViewersId = viewersId;
            }
            // to ensure "username" is required (not null)
            if (username == null)
            {
                throw new InvalidDataException("username is a required property for ApiViewerInfo and cannot be null");
            }
            else
            {
                this.Username = username;
            }
            // to ensure "host" is required (not null)
            if (host == null)
            {
                throw new InvalidDataException("host is a required property for ApiViewerInfo and cannot be null");
            }
            else
            {
                this.Host = host;
            }
            // to ensure "ready" is required (not null)
            if (ready == null)
            {
                throw new InvalidDataException("ready is a required property for ApiViewerInfo and cannot be null");
            }
            else
            {
                this.Ready = ready;
            }
        }
        
        /// <summary>
        /// Gets or Sets ViewersId
        /// </summary>
        [DataMember(Name="viewersId", EmitDefaultValue=false)]
        public string ViewersId { get; set; }

        /// <summary>
        /// Gets or Sets Username
        /// </summary>
        [DataMember(Name="username", EmitDefaultValue=false)]
        public string Username { get; set; }

        /// <summary>
        /// Gets or Sets Host
        /// </summary>
        [DataMember(Name="host", EmitDefaultValue=false)]
        public string Host { get; set; }

        /// <summary>
        /// Gets or Sets Ready
        /// </summary>
        [DataMember(Name="ready", EmitDefaultValue=false)]
        public bool? Ready { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiViewerInfo {\n");
            sb.Append("  ViewersId: ").Append(ViewersId).Append("\n");
            sb.Append("  Username: ").Append(Username).Append("\n");
            sb.Append("  Host: ").Append(Host).Append("\n");
            sb.Append("  Ready: ").Append(Ready).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as ApiViewerInfo);
        }

        /// <summary>
        /// Returns true if ApiViewerInfo instances are equal
        /// </summary>
        /// <param name="input">Instance of ApiViewerInfo to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiViewerInfo input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.ViewersId == input.ViewersId ||
                    (this.ViewersId != null &&
                    this.ViewersId.Equals(input.ViewersId))
                ) && 
                (
                    this.Username == input.Username ||
                    (this.Username != null &&
                    this.Username.Equals(input.Username))
                ) && 
                (
                    this.Host == input.Host ||
                    (this.Host != null &&
                    this.Host.Equals(input.Host))
                ) && 
                (
                    this.Ready == input.Ready ||
                    (this.Ready != null &&
                    this.Ready.Equals(input.Ready))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.ViewersId != null)
                    hashCode = hashCode * 59 + this.ViewersId.GetHashCode();
                if (this.Username != null)
                    hashCode = hashCode * 59 + this.Username.GetHashCode();
                if (this.Host != null)
                    hashCode = hashCode * 59 + this.Host.GetHashCode();
                if (this.Ready != null)
                    hashCode = hashCode * 59 + this.Ready.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
