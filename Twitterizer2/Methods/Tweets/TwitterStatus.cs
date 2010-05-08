﻿//-----------------------------------------------------------------------
// <copyright file="TwitterStatus.cs" company="Patrick 'Ricky' Smith">
//  This file is part of the Twitterizer library (http://code.google.com/p/twitterizer/)
// 
//  Copyright (c) 2010, Patrick "Ricky" Smith (ricky@digitally-born.com)
//  All rights reserved.
//  
//  Redistribution and use in source and binary forms, with or without modification, are 
//  permitted provided that the following conditions are met:
// 
//  - Redistributions of source code must retain the above copyright notice, this list 
//    of conditions and the following disclaimer.
//  - Redistributions in binary form must reproduce the above copyright notice, this list 
//    of conditions and the following disclaimer in the documentation and/or other 
//    materials provided with the distribution.
//  - Neither the name of the Twitterizer nor the names of its contributors may be 
//    used to endorse or promote products derived from this software without specific 
//    prior written permission.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
//  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
//  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
//  IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
//  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
//  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
//  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
//  POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <author>Ricky Smith</author>
// <summary>The TwitterStatus class</summary>
//-----------------------------------------------------------------------
namespace Twitterizer
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;
    using Twitterizer.Core;

    /// <summary>
    /// The TwitterStatus class.
    /// </summary>
    [DataContract]
    [Serializable]
    public class TwitterStatus : TwitterObject
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterStatus"/> class.
        /// </summary>
        internal TwitterStatus() : base() 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterStatus"/> class.
        /// </summary>
        /// <param name="tokens">OAuth access tokens.</param>
        internal TwitterStatus(OAuthTokens tokens) 
            : base()
        {
            this.Tokens = tokens;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the status id.
        /// </summary>
        /// <value>The status id.</value>
        [DataMember(Name = "id")]
        [CLSCompliantAttribute(false)]
        public ulong Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this status message is truncated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this status message is truncated; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "truncated", IsRequired = false)]
        public bool? IsTruncated { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        [DataMember(Name = "created_at")]
        public string CreatedDateString { get; set; }

        /// <summary>
        /// Gets the created date.
        /// </summary>
        /// <value>The created date.</value>
#if !MONO_2_4
        [IgnoreDataMember]
#endif
        public DateTime CreatedDate
        {
            get
            {
                DateTime parsedDate;

                if (DateTime.TryParseExact(
                        this.CreatedDateString,
                        DateFormat, 
                        CultureInfo.InvariantCulture, 
                        DateTimeStyles.None, 
                        out parsedDate))
                {
                    return parsedDate;
                }
                else
                {
                    return new DateTime();
                }
            }
#if MONO_2_4
            set { }
#endif
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        [DataMember(Name = "source")]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the screenName the status is in reply to.
        /// </summary>
        /// <value>The screenName.</value>
        [DataMember(Name = "in_reply_to_screen_name")]
        public string InReplyToScreenName { get; set; }

        /// <summary>
        /// Gets or sets the user id the status is in reply to.
        /// </summary>
        /// <value>The user id.</value>
        [DataMember(Name = "in_reply_to_user_id")]
        [CLSCompliant(false)]
        public ulong? InReplyToUserId { get; set; }

        /// <summary>
        /// Gets or sets the status id the status is in reply to.
        /// </summary>
        /// <value>The status id.</value>
        [DataMember(Name = "in_reply_to_status_id")]
        [CLSCompliant(false)]
        public ulong? InReplyToStatusId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the authenticated user has favorited this status.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is favorited; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "favorited", IsRequired = false)]
        public bool? IsFavorited { get; set; }

        /// <summary>
        /// Gets or sets the text of the status.
        /// </summary>
        /// <value>The status text.</value>
        [DataMember(Name = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user that posted this status.</value>
        [DataMember(Name = "user")]
        public TwitterUser User { get; set; }

        /// <summary>
        /// Gets or sets the retweeted status.
        /// </summary>
        /// <value>The retweeted status.</value>
        [DataMember(Name = "retweeted_status")]
        public TwitterStatus RetweetedStatus { get; set; }
        #endregion

        /// <summary>
        /// Updates the authenticated user's status to the supplied text.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <param name="text">The status text.</param>
        /// <returns>A <see cref="TwitterStatus"/> object of the newly created status.</returns>
        public static TwitterStatus Update(OAuthTokens tokens, string text)
        {
            return Update(tokens, text, null);
        }

        /// <summary>
        /// Updates the specified tokens.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <param name="text">The status text.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// A <see cref="TwitterStatus"/> object of the newly created status.
        /// </returns>
        public static TwitterStatus Update(OAuthTokens tokens, string text, StatusUpdateOptions options)
        {
            return PerformCommand<TwitterStatus>(new Commands.UpdateStatusCommand(tokens, text, options));
        }

        /// <summary>
        /// Deletes the specified tokens.
        /// </summary>
        /// <param name="tokens">The oauth tokens.</param>
        /// <param name="id">The status id.</param>
        /// <returns>A <see cref="TwitterStatus"/> object of the deleted status.</returns>
        [CLSCompliant(false)]
        public static TwitterStatus Delete(OAuthTokens tokens, ulong id)
        {
            Commands.DeleteStatusCommand command = new Twitterizer.Commands.DeleteStatusCommand(tokens, id);

            return PerformCommand<TwitterStatus>(command);
        }

        /// <summary>
        /// Returns a single status, with user information, specified by the id parameter.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <param name="statusId">The status id.</param>
        /// <returns>A <see cref="TwitterStatus"/> instance.</returns>
        [CLSCompliant(false)]
        public static TwitterStatus Show(OAuthTokens tokens, ulong statusId)
        {
            return PerformCommand<TwitterStatus>(new Commands.ShowStatusCommand(tokens, statusId));
        }

        /// <summary>
        /// Returns a single status, with user information, specified by the id parameter.
        /// </summary>
        /// <param name="statusId">The status id.</param>
        /// <returns>A <see cref="TwitterStatus"/> instance.</returns>
        [CLSCompliant(false)]
        public static TwitterStatus Show(ulong statusId)
        {
            return Show(null, statusId);
        } 

        /// <summary>
        /// Retweets a tweet. Requires the id parameter of the tweet you are retweeting. (say that 5 times fast)
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <param name="statusId">The status id.</param>
        /// <returns>A <see cref="TwitterStatus"/> instance.</returns>
        [CLSCompliant(false)]
        public static TwitterStatus Retweet(OAuthTokens tokens, ulong statusId)
        {
            return PerformCommand<TwitterStatus>(new Commands.RetweetCommand(tokens) { StatusId = statusId });
        }

        /// <summary>
        /// Returns up to 100 of the first retweets of a given tweet.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <param name="statusId">The status id.</param>
        /// <param name="count">The count.</param>
        /// <returns>A <see cref="TwitterStatusCollection"/> instance.</returns>
        [CLSCompliant(false)]
        public static TwitterStatusCollection Retweets(OAuthTokens tokens, ulong statusId, int count)
        {
            return PerformCommand<TwitterStatusCollection>(new Commands.RetweetsCommand(tokens, statusId) { Count = count });
        }

        /// <summary>
        /// Returns up to 100 of the first retweets of a given tweet.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <param name="statusId">The status id.</param>
        /// <returns>A <see cref="TwitterStatusCollection"/> instance.</returns>
        [CLSCompliant(false)]
        public static TwitterStatusCollection Retweets(OAuthTokens tokens, ulong statusId)
        {
            return Retweets(tokens, statusId, -1);
        } 
    }
}
