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
    /// CurrentTime
    /// </summary>
    [DataContract]
        public partial class CurrentTime :  IEquatable<CurrentTime>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentTime" /> class.
        /// </summary>
        /// <param name="timeStamp">timeStamp (required).</param>
        public CurrentTime(long? timeStamp = default(long?))
        {
            // to ensure "timeStamp" is required (not null)
            if (timeStamp == null)
            {
                throw new InvalidDataException("timeStamp is a required property for CurrentTime and cannot be null");
            }
            else
            {
                this.TimeStamp = timeStamp;
            }
        }
        
        /// <summary>
        /// Gets or Sets TimeStamp
        /// </summary>
        [DataMember(Name="timeStamp", EmitDefaultValue=false)]
        public long? TimeStamp { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CurrentTime {\n");
            sb.Append("  TimeStamp: ").Append(TimeStamp).Append("\n");
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
            return this.Equals(input as CurrentTime);
        }

        /// <summary>
        /// Returns true if CurrentTime instances are equal
        /// </summary>
        /// <param name="input">Instance of CurrentTime to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CurrentTime input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.TimeStamp == input.TimeStamp ||
                    (this.TimeStamp != null &&
                    this.TimeStamp.Equals(input.TimeStamp))
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
                if (this.TimeStamp != null)
                    hashCode = hashCode * 59 + this.TimeStamp.GetHashCode();
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
