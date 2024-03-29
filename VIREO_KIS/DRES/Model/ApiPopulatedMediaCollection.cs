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
    /// ApiPopulatedMediaCollection
    /// </summary>
    [DataContract]
        public partial class ApiPopulatedMediaCollection :  IEquatable<ApiPopulatedMediaCollection>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiPopulatedMediaCollection" /> class.
        /// </summary>
        /// <param name="collection">collection (required).</param>
        /// <param name="items">items (required).</param>
        public ApiPopulatedMediaCollection(ApiMediaCollection collection = default(ApiMediaCollection), List<ApiMediaItem> items = default(List<ApiMediaItem>))
        {
            // to ensure "collection" is required (not null)
            if (collection == null)
            {
                throw new InvalidDataException("collection is a required property for ApiPopulatedMediaCollection and cannot be null");
            }
            else
            {
                this.Collection = collection;
            }
            // to ensure "items" is required (not null)
            if (items == null)
            {
                throw new InvalidDataException("items is a required property for ApiPopulatedMediaCollection and cannot be null");
            }
            else
            {
                this.Items = items;
            }
        }
        
        /// <summary>
        /// Gets or Sets Collection
        /// </summary>
        [DataMember(Name="collection", EmitDefaultValue=false)]
        public ApiMediaCollection Collection { get; set; }

        /// <summary>
        /// Gets or Sets Items
        /// </summary>
        [DataMember(Name="items", EmitDefaultValue=false)]
        public List<ApiMediaItem> Items { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiPopulatedMediaCollection {\n");
            sb.Append("  Collection: ").Append(Collection).Append("\n");
            sb.Append("  Items: ").Append(Items).Append("\n");
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
            return this.Equals(input as ApiPopulatedMediaCollection);
        }

        /// <summary>
        /// Returns true if ApiPopulatedMediaCollection instances are equal
        /// </summary>
        /// <param name="input">Instance of ApiPopulatedMediaCollection to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiPopulatedMediaCollection input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Collection == input.Collection ||
                    (this.Collection != null &&
                    this.Collection.Equals(input.Collection))
                ) && 
                (
                    this.Items == input.Items ||
                    this.Items != null &&
                    input.Items != null &&
                    this.Items.SequenceEqual(input.Items)
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
                if (this.Collection != null)
                    hashCode = hashCode * 59 + this.Collection.GetHashCode();
                if (this.Items != null)
                    hashCode = hashCode * 59 + this.Items.GetHashCode();
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
