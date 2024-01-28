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
    /// ApiEvaluationTemplateOverview
    /// </summary>
    [DataContract]
        public partial class ApiEvaluationTemplateOverview :  IEquatable<ApiEvaluationTemplateOverview>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEvaluationTemplateOverview" /> class.
        /// </summary>
        /// <param name="id">id (required).</param>
        /// <param name="name">name (required).</param>
        /// <param name="description">description.</param>
        /// <param name="taskCount">taskCount (required).</param>
        /// <param name="teamCount">teamCount (required).</param>
        public ApiEvaluationTemplateOverview(string id = default(string), string name = default(string), string description = default(string), int? taskCount = default(int?), int? teamCount = default(int?))
        {
            // to ensure "id" is required (not null)
            if (id == null)
            {
                throw new InvalidDataException("id is a required property for ApiEvaluationTemplateOverview and cannot be null");
            }
            else
            {
                this.Id = id;
            }
            // to ensure "name" is required (not null)
            if (name == null)
            {
                throw new InvalidDataException("name is a required property for ApiEvaluationTemplateOverview and cannot be null");
            }
            else
            {
                this.Name = name;
            }
            // to ensure "taskCount" is required (not null)
            if (taskCount == null)
            {
                throw new InvalidDataException("taskCount is a required property for ApiEvaluationTemplateOverview and cannot be null");
            }
            else
            {
                this.TaskCount = taskCount;
            }
            // to ensure "teamCount" is required (not null)
            if (teamCount == null)
            {
                throw new InvalidDataException("teamCount is a required property for ApiEvaluationTemplateOverview and cannot be null");
            }
            else
            {
                this.TeamCount = teamCount;
            }
            this.Description = description;
        }
        
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets TaskCount
        /// </summary>
        [DataMember(Name="taskCount", EmitDefaultValue=false)]
        public int? TaskCount { get; set; }

        /// <summary>
        /// Gets or Sets TeamCount
        /// </summary>
        [DataMember(Name="teamCount", EmitDefaultValue=false)]
        public int? TeamCount { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiEvaluationTemplateOverview {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  TaskCount: ").Append(TaskCount).Append("\n");
            sb.Append("  TeamCount: ").Append(TeamCount).Append("\n");
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
            return this.Equals(input as ApiEvaluationTemplateOverview);
        }

        /// <summary>
        /// Returns true if ApiEvaluationTemplateOverview instances are equal
        /// </summary>
        /// <param name="input">Instance of ApiEvaluationTemplateOverview to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiEvaluationTemplateOverview input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                    this.Id.Equals(input.Id))
                ) && 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) && 
                (
                    this.TaskCount == input.TaskCount ||
                    (this.TaskCount != null &&
                    this.TaskCount.Equals(input.TaskCount))
                ) && 
                (
                    this.TeamCount == input.TeamCount ||
                    (this.TeamCount != null &&
                    this.TeamCount.Equals(input.TeamCount))
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
                if (this.Id != null)
                    hashCode = hashCode * 59 + this.Id.GetHashCode();
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.Description != null)
                    hashCode = hashCode * 59 + this.Description.GetHashCode();
                if (this.TaskCount != null)
                    hashCode = hashCode * 59 + this.TaskCount.GetHashCode();
                if (this.TeamCount != null)
                    hashCode = hashCode * 59 + this.TeamCount.GetHashCode();
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
