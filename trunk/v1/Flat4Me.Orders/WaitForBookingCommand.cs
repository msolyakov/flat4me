﻿using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flat4Me.Activities.Data;

namespace Flat4Me.Orders
{
    public class WaitForBookingCommand : NativeActivity<object>
    {
        // Define an activity input argument
        public InArgument<BookingCommand> Command { get; set; }

        /// <summary>
        /// When implemented in a derived class, runs the activity’s execution logic.
        /// </summary>
        /// <param name="context">The execution context in which the activity executes.</param>
        protected override void Execute(NativeActivityContext context)
        {
            context.CreateBookmark(this.Command.Get(context).ToString(),
                (activityContext, bookmark, value) => activityContext.SetValue(this.Result, value));

        }

        /// <summary>
        ///   Gets a value indicating whether CanInduceIdle.
        /// </summary>
        protected override bool CanInduceIdle
        {
            get
            {
                return true;
            }
        }
    }
}