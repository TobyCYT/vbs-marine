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
    /// Defines ApiTeamAggregatorType
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
        public enum ApiTeamAggregatorType
    {
        /// <summary>
        /// Enum MIN for value: MIN
        /// </summary>
        [EnumMember(Value = "MIN")]
        MIN = 1,
        /// <summary>
        /// Enum MAX for value: MAX
        /// </summary>
        [EnumMember(Value = "MAX")]
        MAX = 2,
        /// <summary>
        /// Enum MEAN for value: MEAN
        /// </summary>
        [EnumMember(Value = "MEAN")]
        MEAN = 3,
        /// <summary>
        /// Enum LAST for value: LAST
        /// </summary>
        [EnumMember(Value = "LAST")]
        LAST = 4    }
}
