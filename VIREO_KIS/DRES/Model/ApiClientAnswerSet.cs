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
    /// ApiClientAnswerSet
    /// </summary>
    [DataContract]
        public partial class ApiClientAnswerSet :  IEquatable<ApiClientAnswerSet>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClientAnswerSet" /> class.
        /// </summary>
        /// <param name="taskId">taskId.</param>
        /// <param name="taskName">taskName.</param>
        /// <param name="answers">answers (required).</param>
        public ApiClientAnswerSet(string taskId = default(string), string taskName = default(string), List<ApiClientAnswer> answers = default(List<ApiClientAnswer>))
        {
            // to ensure "answers" is required (not null)
            if (answers == null)
            {
                throw new InvalidDataException("answers is a required property for ApiClientAnswerSet and cannot be null");
            }
            else
            {
                this.Answers = answers;
            }
            this.TaskId = taskId;
            this.TaskName = taskName;
        }
        
        /// <summary>
        /// Gets or Sets TaskId
        /// </summary>
        [DataMember(Name="taskId", EmitDefaultValue=false)]
        public string TaskId { get; set; }

        /// <summary>
        /// Gets or Sets TaskName
        /// </summary>
        [DataMember(Name="taskName", EmitDefaultValue=false)]
        public string TaskName { get; set; }

        /// <summary>
        /// Gets or Sets Answers
        /// </summary>
        [DataMember(Name="answers", EmitDefaultValue=false)]
        public List<ApiClientAnswer> Answers { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiClientAnswerSet {\n");
            sb.Append("  TaskId: ").Append(TaskId).Append("\n");
            sb.Append("  TaskName: ").Append(TaskName).Append("\n");
            sb.Append("  Answers: ").Append(Answers).Append("\n");
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
            return this.Equals(input as ApiClientAnswerSet);
        }

        /// <summary>
        /// Returns true if ApiClientAnswerSet instances are equal
        /// </summary>
        /// <param name="input">Instance of ApiClientAnswerSet to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiClientAnswerSet input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.TaskId == input.TaskId ||
                    (this.TaskId != null &&
                    this.TaskId.Equals(input.TaskId))
                ) && 
                (
                    this.TaskName == input.TaskName ||
                    (this.TaskName != null &&
                    this.TaskName.Equals(input.TaskName))
                ) && 
                (
                    this.Answers == input.Answers ||
                    this.Answers != null &&
                    input.Answers != null &&
                    this.Answers.SequenceEqual(input.Answers)
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
                if (this.TaskId != null)
                    hashCode = hashCode * 59 + this.TaskId.GetHashCode();
                if (this.TaskName != null)
                    hashCode = hashCode * 59 + this.TaskName.GetHashCode();
                if (this.Answers != null)
                    hashCode = hashCode * 59 + this.Answers.GetHashCode();
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
