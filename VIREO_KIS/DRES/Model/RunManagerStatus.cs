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
    /// Defines RunManagerStatus
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
        public enum RunManagerStatus
    {
        /// <summary>
        /// Enum CREATED for value: CREATED
        /// </summary>
        [EnumMember(Value = "CREATED")]
        CREATED = 1,
        /// <summary>
        /// Enum ACTIVE for value: ACTIVE
        /// </summary>
        [EnumMember(Value = "ACTIVE")]
        ACTIVE = 2,
        /// <summary>
        /// Enum TERMINATED for value: TERMINATED
        /// </summary>
        [EnumMember(Value = "TERMINATED")]
        TERMINATED = 3    }
}
