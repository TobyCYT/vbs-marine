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
    /// Defines ApiHintOption
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
        public enum ApiHintOption
    {
        /// <summary>
        /// Enum IMAGEITEM for value: IMAGE_ITEM
        /// </summary>
        [EnumMember(Value = "IMAGE_ITEM")]
        IMAGEITEM = 1,
        /// <summary>
        /// Enum VIDEOITEMSEGMENT for value: VIDEO_ITEM_SEGMENT
        /// </summary>
        [EnumMember(Value = "VIDEO_ITEM_SEGMENT")]
        VIDEOITEMSEGMENT = 2,
        /// <summary>
        /// Enum TEXT for value: TEXT
        /// </summary>
        [EnumMember(Value = "TEXT")]
        TEXT = 3,
        /// <summary>
        /// Enum EXTERNALIMAGE for value: EXTERNAL_IMAGE
        /// </summary>
        [EnumMember(Value = "EXTERNAL_IMAGE")]
        EXTERNALIMAGE = 4,
        /// <summary>
        /// Enum EXTERNALVIDEO for value: EXTERNAL_VIDEO
        /// </summary>
        [EnumMember(Value = "EXTERNAL_VIDEO")]
        EXTERNALVIDEO = 5    }
}
