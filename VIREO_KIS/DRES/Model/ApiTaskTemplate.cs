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
    /// ApiTaskTemplate
    /// </summary>
    [DataContract]
        public partial class ApiTaskTemplate :  IEquatable<ApiTaskTemplate>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTaskTemplate" /> class.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="name">name (required).</param>
        /// <param name="taskGroup">taskGroup (required).</param>
        /// <param name="taskType">taskType (required).</param>
        /// <param name="duration">duration (required).</param>
        /// <param name="collectionId">collectionId (required).</param>
        /// <param name="targets">targets (required).</param>
        /// <param name="hints">hints (required).</param>
        /// <param name="comment">comment.</param>
        public ApiTaskTemplate(string id = default(string), string name = default(string), string taskGroup = default(string), string taskType = default(string), long? duration = default(long?), string collectionId = default(string), List<ApiTarget> targets = default(List<ApiTarget>), List<ApiHint> hints = default(List<ApiHint>), string comment = default(string))
        {
            // to ensure "name" is required (not null)
            if (name == null)
            {
                throw new InvalidDataException("name is a required property for ApiTaskTemplate and cannot be null");
            }
            else
            {
                this.Name = name;
            }
            // to ensure "taskGroup" is required (not null)
            if (taskGroup == null)
            {
                throw new InvalidDataException("taskGroup is a required property for ApiTaskTemplate and cannot be null");
            }
            else
            {
                this.TaskGroup = taskGroup;
            }
            // to ensure "taskType" is required (not null)
            if (taskType == null)
            {
                throw new InvalidDataException("taskType is a required property for ApiTaskTemplate and cannot be null");
            }
            else
            {
                this.TaskType = taskType;
            }
            // to ensure "duration" is required (not null)
            if (duration == null)
            {
                throw new InvalidDataException("duration is a required property for ApiTaskTemplate and cannot be null");
            }
            else
            {
                this.Duration = duration;
            }
            // to ensure "collectionId" is required (not null)
            if (collectionId == null)
            {
                throw new InvalidDataException("collectionId is a required property for ApiTaskTemplate and cannot be null");
            }
            else
            {
                this.CollectionId = collectionId;
            }
            // to ensure "targets" is required (not null)
            if (targets == null)
            {
                throw new InvalidDataException("targets is a required property for ApiTaskTemplate and cannot be null");
            }
            else
            {
                this.Targets = targets;
            }
            // to ensure "hints" is required (not null)
            if (hints == null)
            {
                throw new InvalidDataException("hints is a required property for ApiTaskTemplate and cannot be null");
            }
            else
            {
                this.Hints = hints;
            }
            this.Id = id;
            this.Comment = comment;
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
        /// Gets or Sets TaskGroup
        /// </summary>
        [DataMember(Name="taskGroup", EmitDefaultValue=false)]
        public string TaskGroup { get; set; }

        /// <summary>
        /// Gets or Sets TaskType
        /// </summary>
        [DataMember(Name="taskType", EmitDefaultValue=false)]
        public string TaskType { get; set; }

        /// <summary>
        /// Gets or Sets Duration
        /// </summary>
        [DataMember(Name="duration", EmitDefaultValue=false)]
        public long? Duration { get; set; }

        /// <summary>
        /// Gets or Sets CollectionId
        /// </summary>
        [DataMember(Name="collectionId", EmitDefaultValue=false)]
        public string CollectionId { get; set; }

        /// <summary>
        /// Gets or Sets Targets
        /// </summary>
        [DataMember(Name="targets", EmitDefaultValue=false)]
        public List<ApiTarget> Targets { get; set; }

        /// <summary>
        /// Gets or Sets Hints
        /// </summary>
        [DataMember(Name="hints", EmitDefaultValue=false)]
        public List<ApiHint> Hints { get; set; }

        /// <summary>
        /// Gets or Sets Comment
        /// </summary>
        [DataMember(Name="comment", EmitDefaultValue=false)]
        public string Comment { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiTaskTemplate {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  TaskGroup: ").Append(TaskGroup).Append("\n");
            sb.Append("  TaskType: ").Append(TaskType).Append("\n");
            sb.Append("  Duration: ").Append(Duration).Append("\n");
            sb.Append("  CollectionId: ").Append(CollectionId).Append("\n");
            sb.Append("  Targets: ").Append(Targets).Append("\n");
            sb.Append("  Hints: ").Append(Hints).Append("\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
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
            return this.Equals(input as ApiTaskTemplate);
        }

        /// <summary>
        /// Returns true if ApiTaskTemplate instances are equal
        /// </summary>
        /// <param name="input">Instance of ApiTaskTemplate to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiTaskTemplate input)
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
                    this.TaskGroup == input.TaskGroup ||
                    (this.TaskGroup != null &&
                    this.TaskGroup.Equals(input.TaskGroup))
                ) && 
                (
                    this.TaskType == input.TaskType ||
                    (this.TaskType != null &&
                    this.TaskType.Equals(input.TaskType))
                ) && 
                (
                    this.Duration == input.Duration ||
                    (this.Duration != null &&
                    this.Duration.Equals(input.Duration))
                ) && 
                (
                    this.CollectionId == input.CollectionId ||
                    (this.CollectionId != null &&
                    this.CollectionId.Equals(input.CollectionId))
                ) && 
                (
                    this.Targets == input.Targets ||
                    this.Targets != null &&
                    input.Targets != null &&
                    this.Targets.SequenceEqual(input.Targets)
                ) && 
                (
                    this.Hints == input.Hints ||
                    this.Hints != null &&
                    input.Hints != null &&
                    this.Hints.SequenceEqual(input.Hints)
                ) && 
                (
                    this.Comment == input.Comment ||
                    (this.Comment != null &&
                    this.Comment.Equals(input.Comment))
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
                if (this.TaskGroup != null)
                    hashCode = hashCode * 59 + this.TaskGroup.GetHashCode();
                if (this.TaskType != null)
                    hashCode = hashCode * 59 + this.TaskType.GetHashCode();
                if (this.Duration != null)
                    hashCode = hashCode * 59 + this.Duration.GetHashCode();
                if (this.CollectionId != null)
                    hashCode = hashCode * 59 + this.CollectionId.GetHashCode();
                if (this.Targets != null)
                    hashCode = hashCode * 59 + this.Targets.GetHashCode();
                if (this.Hints != null)
                    hashCode = hashCode * 59 + this.Hints.GetHashCode();
                if (this.Comment != null)
                    hashCode = hashCode * 59 + this.Comment.GetHashCode();
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