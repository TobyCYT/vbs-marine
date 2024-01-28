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
    /// ApiJudgement
    /// </summary>
    [DataContract]
        public partial class ApiJudgement :  IEquatable<ApiJudgement>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiJudgement" /> class.
        /// </summary>
        /// <param name="token">token (required).</param>
        /// <param name="validator">validator (required).</param>
        /// <param name="verdict">verdict (required).</param>
        public ApiJudgement(string token = default(string), string validator = default(string), ApiVerdictStatus verdict = default(ApiVerdictStatus))
        {
            // to ensure "token" is required (not null)
            if (token == null)
            {
                throw new InvalidDataException("token is a required property for ApiJudgement and cannot be null");
            }
            else
            {
                this.Token = token;
            }
            // to ensure "validator" is required (not null)
            if (validator == null)
            {
                throw new InvalidDataException("validator is a required property for ApiJudgement and cannot be null");
            }
            else
            {
                this.Validator = validator;
            }
            // to ensure "verdict" is required (not null)
            if (verdict == null)
            {
                throw new InvalidDataException("verdict is a required property for ApiJudgement and cannot be null");
            }
            else
            {
                this.Verdict = verdict;
            }
        }
        
        /// <summary>
        /// Gets or Sets Token
        /// </summary>
        [DataMember(Name="token", EmitDefaultValue=false)]
        public string Token { get; set; }

        /// <summary>
        /// Gets or Sets Validator
        /// </summary>
        [DataMember(Name="validator", EmitDefaultValue=false)]
        public string Validator { get; set; }

        /// <summary>
        /// Gets or Sets Verdict
        /// </summary>
        [DataMember(Name="verdict", EmitDefaultValue=false)]
        public ApiVerdictStatus Verdict { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiJudgement {\n");
            sb.Append("  Token: ").Append(Token).Append("\n");
            sb.Append("  Validator: ").Append(Validator).Append("\n");
            sb.Append("  Verdict: ").Append(Verdict).Append("\n");
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
            return this.Equals(input as ApiJudgement);
        }

        /// <summary>
        /// Returns true if ApiJudgement instances are equal
        /// </summary>
        /// <param name="input">Instance of ApiJudgement to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiJudgement input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Token == input.Token ||
                    (this.Token != null &&
                    this.Token.Equals(input.Token))
                ) && 
                (
                    this.Validator == input.Validator ||
                    (this.Validator != null &&
                    this.Validator.Equals(input.Validator))
                ) && 
                (
                    this.Verdict == input.Verdict ||
                    (this.Verdict != null &&
                    this.Verdict.Equals(input.Verdict))
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
                if (this.Token != null)
                    hashCode = hashCode * 59 + this.Token.GetHashCode();
                if (this.Validator != null)
                    hashCode = hashCode * 59 + this.Validator.GetHashCode();
                if (this.Verdict != null)
                    hashCode = hashCode * 59 + this.Verdict.GetHashCode();
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
