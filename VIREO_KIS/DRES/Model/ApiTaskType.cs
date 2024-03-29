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
    /// ApiTaskType
    /// </summary>
    [DataContract]
        public partial class ApiTaskType :  IEquatable<ApiTaskType>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTaskType" /> class.
        /// </summary>
        /// <param name="name">name (required).</param>
        /// <param name="duration">duration (required).</param>
        /// <param name="targetOption">targetOption (required).</param>
        /// <param name="hintOptions">hintOptions (required).</param>
        /// <param name="submissionOptions">submissionOptions (required).</param>
        /// <param name="taskOptions">taskOptions (required).</param>
        /// <param name="scoreOption">scoreOption (required).</param>
        /// <param name="configuration">configuration (required).</param>
        public ApiTaskType(string name = default(string), long? duration = default(long?), ApiTargetOption targetOption = default(ApiTargetOption), List<ApiHintOption> hintOptions = default(List<ApiHintOption>), List<ApiSubmissionOption> submissionOptions = default(List<ApiSubmissionOption>), List<ApiTaskOption> taskOptions = default(List<ApiTaskOption>), ApiScoreOption scoreOption = default(ApiScoreOption), Dictionary<string, string> configuration = default(Dictionary<string, string>))
        {
            // to ensure "name" is required (not null)
            if (name == null)
            {
                throw new InvalidDataException("name is a required property for ApiTaskType and cannot be null");
            }
            else
            {
                this.Name = name;
            }
            // to ensure "duration" is required (not null)
            if (duration == null)
            {
                throw new InvalidDataException("duration is a required property for ApiTaskType and cannot be null");
            }
            else
            {
                this.Duration = duration;
            }
            // to ensure "targetOption" is required (not null)
            if (targetOption == null)
            {
                throw new InvalidDataException("targetOption is a required property for ApiTaskType and cannot be null");
            }
            else
            {
                this.TargetOption = targetOption;
            }
            // to ensure "hintOptions" is required (not null)
            if (hintOptions == null)
            {
                throw new InvalidDataException("hintOptions is a required property for ApiTaskType and cannot be null");
            }
            else
            {
                this.HintOptions = hintOptions;
            }
            // to ensure "submissionOptions" is required (not null)
            if (submissionOptions == null)
            {
                throw new InvalidDataException("submissionOptions is a required property for ApiTaskType and cannot be null");
            }
            else
            {
                this.SubmissionOptions = submissionOptions;
            }
            // to ensure "taskOptions" is required (not null)
            if (taskOptions == null)
            {
                throw new InvalidDataException("taskOptions is a required property for ApiTaskType and cannot be null");
            }
            else
            {
                this.TaskOptions = taskOptions;
            }
            // to ensure "scoreOption" is required (not null)
            if (scoreOption == null)
            {
                throw new InvalidDataException("scoreOption is a required property for ApiTaskType and cannot be null");
            }
            else
            {
                this.ScoreOption = scoreOption;
            }
            // to ensure "configuration" is required (not null)
            if (configuration == null)
            {
                throw new InvalidDataException("configuration is a required property for ApiTaskType and cannot be null");
            }
            else
            {
                this.Configuration = configuration;
            }
        }
        
        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Duration
        /// </summary>
        [DataMember(Name="duration", EmitDefaultValue=false)]
        public long? Duration { get; set; }

        /// <summary>
        /// Gets or Sets TargetOption
        /// </summary>
        [DataMember(Name="targetOption", EmitDefaultValue=false)]
        public ApiTargetOption TargetOption { get; set; }

        /// <summary>
        /// Gets or Sets HintOptions
        /// </summary>
        [DataMember(Name="hintOptions", EmitDefaultValue=false)]
        public List<ApiHintOption> HintOptions { get; set; }

        /// <summary>
        /// Gets or Sets SubmissionOptions
        /// </summary>
        [DataMember(Name="submissionOptions", EmitDefaultValue=false)]
        public List<ApiSubmissionOption> SubmissionOptions { get; set; }

        /// <summary>
        /// Gets or Sets TaskOptions
        /// </summary>
        [DataMember(Name="taskOptions", EmitDefaultValue=false)]
        public List<ApiTaskOption> TaskOptions { get; set; }

        /// <summary>
        /// Gets or Sets ScoreOption
        /// </summary>
        [DataMember(Name="scoreOption", EmitDefaultValue=false)]
        public ApiScoreOption ScoreOption { get; set; }

        /// <summary>
        /// Gets or Sets Configuration
        /// </summary>
        [DataMember(Name="configuration", EmitDefaultValue=false)]
        public Dictionary<string, string> Configuration { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiTaskType {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Duration: ").Append(Duration).Append("\n");
            sb.Append("  TargetOption: ").Append(TargetOption).Append("\n");
            sb.Append("  HintOptions: ").Append(HintOptions).Append("\n");
            sb.Append("  SubmissionOptions: ").Append(SubmissionOptions).Append("\n");
            sb.Append("  TaskOptions: ").Append(TaskOptions).Append("\n");
            sb.Append("  ScoreOption: ").Append(ScoreOption).Append("\n");
            sb.Append("  Configuration: ").Append(Configuration).Append("\n");
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
            return this.Equals(input as ApiTaskType);
        }

        /// <summary>
        /// Returns true if ApiTaskType instances are equal
        /// </summary>
        /// <param name="input">Instance of ApiTaskType to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiTaskType input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.Duration == input.Duration ||
                    (this.Duration != null &&
                    this.Duration.Equals(input.Duration))
                ) && 
                (
                    this.TargetOption == input.TargetOption ||
                    (this.TargetOption != null &&
                    this.TargetOption.Equals(input.TargetOption))
                ) && 
                (
                    this.HintOptions == input.HintOptions ||
                    this.HintOptions != null &&
                    input.HintOptions != null &&
                    this.HintOptions.SequenceEqual(input.HintOptions)
                ) && 
                (
                    this.SubmissionOptions == input.SubmissionOptions ||
                    this.SubmissionOptions != null &&
                    input.SubmissionOptions != null &&
                    this.SubmissionOptions.SequenceEqual(input.SubmissionOptions)
                ) && 
                (
                    this.TaskOptions == input.TaskOptions ||
                    this.TaskOptions != null &&
                    input.TaskOptions != null &&
                    this.TaskOptions.SequenceEqual(input.TaskOptions)
                ) && 
                (
                    this.ScoreOption == input.ScoreOption ||
                    (this.ScoreOption != null &&
                    this.ScoreOption.Equals(input.ScoreOption))
                ) && 
                (
                    this.Configuration == input.Configuration ||
                    this.Configuration != null &&
                    input.Configuration != null &&
                    this.Configuration.SequenceEqual(input.Configuration)
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
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.Duration != null)
                    hashCode = hashCode * 59 + this.Duration.GetHashCode();
                if (this.TargetOption != null)
                    hashCode = hashCode * 59 + this.TargetOption.GetHashCode();
                if (this.HintOptions != null)
                    hashCode = hashCode * 59 + this.HintOptions.GetHashCode();
                if (this.SubmissionOptions != null)
                    hashCode = hashCode * 59 + this.SubmissionOptions.GetHashCode();
                if (this.TaskOptions != null)
                    hashCode = hashCode * 59 + this.TaskOptions.GetHashCode();
                if (this.ScoreOption != null)
                    hashCode = hashCode * 59 + this.ScoreOption.GetHashCode();
                if (this.Configuration != null)
                    hashCode = hashCode * 59 + this.Configuration.GetHashCode();
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
