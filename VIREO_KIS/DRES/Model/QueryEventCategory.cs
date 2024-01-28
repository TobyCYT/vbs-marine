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
    /// Defines QueryEventCategory
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
        public enum QueryEventCategory
    {
        /// <summary>
        /// Enum TEXT for value: TEXT
        /// </summary>
        [EnumMember(Value = "TEXT")]
        TEXT = 1,
        /// <summary>
        /// Enum IMAGE for value: IMAGE
        /// </summary>
        [EnumMember(Value = "IMAGE")]
        IMAGE = 2,
        /// <summary>
        /// Enum SKETCH for value: SKETCH
        /// </summary>
        [EnumMember(Value = "SKETCH")]
        SKETCH = 3,
        /// <summary>
        /// Enum FILTER for value: FILTER
        /// </summary>
        [EnumMember(Value = "FILTER")]
        FILTER = 4,
        /// <summary>
        /// Enum BROWSING for value: BROWSING
        /// </summary>
        [EnumMember(Value = "BROWSING")]
        BROWSING = 5,
        /// <summary>
        /// Enum COOPERATION for value: COOPERATION
        /// </summary>
        [EnumMember(Value = "COOPERATION")]
        COOPERATION = 6,
        /// <summary>
        /// Enum OTHER for value: OTHER
        /// </summary>
        [EnumMember(Value = "OTHER")]
        OTHER = 7    }
}