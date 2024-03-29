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
    /// ApiTask
    /// </summary>
    [DataContract]
        public partial class ApiTask :  IEquatable<ApiTask>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTask" /> class.
        /// </summary>
        /// <param name="taskId">taskId (required).</param>
        /// <param name="templateId">templateId (required).</param>
        /// <param name="started">started.</param>
        /// <param name="ended">ended.</param>
        /// <param name="submissions">submissions (required).</param>
        public ApiTask(string taskId = default(string), string templateId = default(string), long? started = default(long?), long? ended = default(long?), List<ApiAnswerSet> submissions = default(List<ApiAnswerSet>))
        {
            // to ensure "taskId" is required (not null)
            if (taskId == null)
            {
                throw new InvalidDataException("taskId is a required property for ApiTask and cannot be null");
            }
            else
            {
                this.TaskId = taskId;
            }
            // to ensure "templateId" is required (not null)
            if (templateId == null)
            {
                throw new InvalidDataException("templateId is a required property for ApiTask and cannot be null");
            }
            else
            {
                this.TemplateId = templateId;
            }
            // to ensure "submissions" is required (not null)
            if (submissions == null)
            {
                throw new InvalidDataException("submissions is a required property for ApiTask and cannot be null");
            }
            else
            {
                this.Submissions = submissions;
            }
            this.Started = started;
            this.Ended = ended;
        }
        
        /// <summary>
        /// Gets or Sets TaskId
        /// </summary>
        [DataMember(Name="taskId", EmitDefaultValue=false)]
        public string TaskId { get; set; }

        /// <summary>
        /// Gets or Sets TemplateId
        /// </summary>
        [DataMember(Name="templateId", EmitDefaultValue=false)]
        public string TemplateId { get; set; }

        /// <summary>
        /// Gets or Sets Started
        /// </summary>
        [DataMember(Name="started", EmitDefaultValue=false)]
        public long? Started { get; set; }

        /// <summary>
        /// Gets or Sets Ended
        /// </summary>
        [DataMember(Name="ended", EmitDefaultValue=false)]
        public long? Ended { get; set; }

        /// <summary>
        /// Gets or Sets Submissions
        /// </summary>
        [DataMember(Name="submissions", EmitDefaultValue=false)]
        public List<ApiAnswerSet> Submissions { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiTask {\n");
            sb.Append("  TaskId: ").Append(TaskId).Append("\n");
            sb.Append("  TemplateId: ").Append(TemplateId).Append("\n");
            sb.Append("  Started: ").Append(Started).Append("\n");
            sb.Append("  Ended: ").Append(Ended).Append("\n");
            sb.Append("  Submissions: ").Append(Submissions).Append("\n");
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
            return this.Equals(input as ApiTask);
        }

        /// <summary>
        /// Returns true if ApiTask instances are equal
        /// </summary>
        /// <param name="input">Instance of ApiTask to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiTask input)
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
                    this.TemplateId == input.TemplateId ||
                    (this.TemplateId != null &&
                    this.TemplateId.Equals(input.TemplateId))
                ) && 
                (
                    this.Started == input.Started ||
                    (this.Started != null &&
                    this.Started.Equals(input.Started))
                ) && 
                (
                    this.Ended == input.Ended ||
                    (this.Ended != null &&
                    this.Ended.Equals(input.Ended))
                ) && 
                (
                    this.Submissions == input.Submissions ||
                    this.Submissions != null &&
                    input.Submissions != null &&
                    this.Submissions.SequenceEqual(input.Submissions)
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
                if (this.TemplateId != null)
                    hashCode = hashCode * 59 + this.TemplateId.GetHashCode();
                if (this.Started != null)
                    hashCode = hashCode * 59 + this.Started.GetHashCode();
                if (this.Ended != null)
                    hashCode = hashCode * 59 + this.Ended.GetHashCode();
                if (this.Submissions != null)
                    hashCode = hashCode * 59 + this.Submissions.GetHashCode();
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
