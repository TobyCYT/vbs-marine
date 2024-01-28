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
    /// ApiEvaluation
    /// </summary>
    [DataContract]
        public partial class ApiEvaluation :  IEquatable<ApiEvaluation>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEvaluation" /> class.
        /// </summary>
        /// <param name="evaluationId">evaluationId (required).</param>
        /// <param name="name">name (required).</param>
        /// <param name="type">type (required).</param>
        /// <param name="template">template (required).</param>
        /// <param name="created">created (required).</param>
        /// <param name="started">started.</param>
        /// <param name="ended">ended.</param>
        /// <param name="tasks">tasks (required).</param>
        public ApiEvaluation(string evaluationId = default(string), string name = default(string), ApiEvaluationType type = default(ApiEvaluationType), ApiEvaluationTemplate template = default(ApiEvaluationTemplate), long? created = default(long?), long? started = default(long?), long? ended = default(long?), List<ApiTask> tasks = default(List<ApiTask>))
        {
            // to ensure "evaluationId" is required (not null)
            if (evaluationId == null)
            {
                throw new InvalidDataException("evaluationId is a required property for ApiEvaluation and cannot be null");
            }
            else
            {
                this.EvaluationId = evaluationId;
            }
            // to ensure "name" is required (not null)
            if (name == null)
            {
                throw new InvalidDataException("name is a required property for ApiEvaluation and cannot be null");
            }
            else
            {
                this.Name = name;
            }
            // to ensure "type" is required (not null)
            if (type == null)
            {
                throw new InvalidDataException("type is a required property for ApiEvaluation and cannot be null");
            }
            else
            {
                this.Type = type;
            }
            // to ensure "template" is required (not null)
            if (template == null)
            {
                throw new InvalidDataException("template is a required property for ApiEvaluation and cannot be null");
            }
            else
            {
                this.Template = template;
            }
            // to ensure "created" is required (not null)
            if (created == null)
            {
                throw new InvalidDataException("created is a required property for ApiEvaluation and cannot be null");
            }
            else
            {
                this.Created = created;
            }
            // to ensure "tasks" is required (not null)
            if (tasks == null)
            {
                throw new InvalidDataException("tasks is a required property for ApiEvaluation and cannot be null");
            }
            else
            {
                this.Tasks = tasks;
            }
            this.Started = started;
            this.Ended = ended;
        }
        
        /// <summary>
        /// Gets or Sets EvaluationId
        /// </summary>
        [DataMember(Name="evaluationId", EmitDefaultValue=false)]
        public string EvaluationId { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public ApiEvaluationType Type { get; set; }

        /// <summary>
        /// Gets or Sets Template
        /// </summary>
        [DataMember(Name="template", EmitDefaultValue=false)]
        public ApiEvaluationTemplate Template { get; set; }

        /// <summary>
        /// Gets or Sets Created
        /// </summary>
        [DataMember(Name="created", EmitDefaultValue=false)]
        public long? Created { get; set; }

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
        /// Gets or Sets Tasks
        /// </summary>
        [DataMember(Name="tasks", EmitDefaultValue=false)]
        public List<ApiTask> Tasks { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiEvaluation {\n");
            sb.Append("  EvaluationId: ").Append(EvaluationId).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Template: ").Append(Template).Append("\n");
            sb.Append("  Created: ").Append(Created).Append("\n");
            sb.Append("  Started: ").Append(Started).Append("\n");
            sb.Append("  Ended: ").Append(Ended).Append("\n");
            sb.Append("  Tasks: ").Append(Tasks).Append("\n");
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
            return this.Equals(input as ApiEvaluation);
        }

        /// <summary>
        /// Returns true if ApiEvaluation instances are equal
        /// </summary>
        /// <param name="input">Instance of ApiEvaluation to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiEvaluation input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.EvaluationId == input.EvaluationId ||
                    (this.EvaluationId != null &&
                    this.EvaluationId.Equals(input.EvaluationId))
                ) && 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                ) && 
                (
                    this.Template == input.Template ||
                    (this.Template != null &&
                    this.Template.Equals(input.Template))
                ) && 
                (
                    this.Created == input.Created ||
                    (this.Created != null &&
                    this.Created.Equals(input.Created))
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
                    this.Tasks == input.Tasks ||
                    this.Tasks != null &&
                    input.Tasks != null &&
                    this.Tasks.SequenceEqual(input.Tasks)
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
                if (this.EvaluationId != null)
                    hashCode = hashCode * 59 + this.EvaluationId.GetHashCode();
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.Type != null)
                    hashCode = hashCode * 59 + this.Type.GetHashCode();
                if (this.Template != null)
                    hashCode = hashCode * 59 + this.Template.GetHashCode();
                if (this.Created != null)
                    hashCode = hashCode * 59 + this.Created.GetHashCode();
                if (this.Started != null)
                    hashCode = hashCode * 59 + this.Started.GetHashCode();
                if (this.Ended != null)
                    hashCode = hashCode * 59 + this.Ended.GetHashCode();
                if (this.Tasks != null)
                    hashCode = hashCode * 59 + this.Tasks.GetHashCode();
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