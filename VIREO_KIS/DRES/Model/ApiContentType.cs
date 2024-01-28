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
    /// Defines ApiContentType
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
        public enum ApiContentType
    {
        /// <summary>
        /// Enum EMPTY for value: EMPTY
        /// </summary>
        [EnumMember(Value = "EMPTY")]
        EMPTY = 1,
        /// <summary>
        /// Enum TEXT for value: TEXT
        /// </summary>
        [EnumMember(Value = "TEXT")]
        TEXT = 2,
        /// <summary>
        /// Enum VIDEO for value: VIDEO
        /// </summary>
        [EnumMember(Value = "VIDEO")]
        VIDEO = 3,
        /// <summary>
        /// Enum IMAGE for value: IMAGE
        /// </summary>
        [EnumMember(Value = "IMAGE")]
        IMAGE = 4    }
}
