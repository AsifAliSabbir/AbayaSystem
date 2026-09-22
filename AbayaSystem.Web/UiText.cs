using AbayaSystem.Core;

namespace AbayaSystem.Web;

/// <summary>
/// Presentation-only translations. Domain values, enum names, and database data
/// remain unchanged; this service only controls text rendered by the UI.
/// </summary>
public sealed class UiText
{
    private readonly Dictionary<string, (string English, string Bengali)> _texts =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Workflow"] = ("Workflow", "কাজের ধাপ"),
            ["OrderIntake"] = ("Order Intake", "অর্ডার নেয়া"),
            ["FabricProcurement"] = ("Fabric Procurement", "কাপড় কিনা"),
            ["CuttingDashboard"] = ("Cutting Dashboard", "কাটিং ড্যাশবোর্ড"),
            ["TailorsAssignment"] = ("Tailors Assignment", "টেইলারকে কাজ দেয়া"),
            ["TailorsDashboard"] = ("Tailors Dashboard", "টেইলার ড্যাশবোর্ড"),
            ["ExternalDispatch"] = ("External Dispatch", "বাইরে মাল পাঠানো"),
            ["ExternalVendorReturns"] = ("External Vendor Returns", "বাইরের মাল নেয়া"),
            ["HandEmbroideryAssignment"] = ("Hand Embroidery Assignment", "হাতের কাজ দেয়া"),
            ["HandEmbroidererDashboard"] = ("Hand Embroiderer Dashboard", "হাতের কাজের ড্যাশবোর্ড"),
            ["Overview"] = ("Overview", "সারসংক্ষেপ"),
            ["Dashboard"] = ("Dashboard", "ড্যাশবোর্ড"),
            ["DashboardDescription"] = ("A live overview of orders, production, delivery, and collections", "অর্ডার, উৎপাদন, ডেলিভারি ও আদায়ের সামগ্রিক চিত্র"),
            ["AssignedItems"] = ("Assigned Items", "দেয়া কাজের লিস্ট"),
            ["OrdersDescription"] = ("Search, filter, track status, and manage all customer orders", "সব ক্রেতার অর্ডার খুঁজুন, ফিল্টার করুন, অবস্থা দেখুন ও পরিচালনা করুন"),
            ["CreateNewOrder"] = ("Create New Order", "নতুন অর্ডার এন্ট্রি"),
            ["FilterSearchOptions"] = ("Filter & Search Options", "ফিল্টার ও অনুসন্ধানের বিকল্প"),
            ["ResetFilters"] = ("Reset Filters", "ফিল্টার রিসেট করুন"),
            ["SearchTicket"] = ("Search Order No", "অর্ডার নম্বর খুঁজুন"),
            ["SearchCustomer"] = ("Search Customer...", "কাস্টমার খুঁজুন..."),
            ["SearchPhone"] = ("Search Phone...", "ফোন নম্বর খুঁজুন..."),
            ["AllStates"] = ("All States", "সব অবস্থা"),
            ["SortByUrgentFirst"] = ("Sort By (Urgent First)", "সাজিয়ে নিন (আর্জেন্ট আগে)"),
            ["UrgentThenDelivery"] = ("Urgent first, earliest delivery date", "জরুরি আগে, সবচেয়ে আগের ডেলিভারির তারিখ আগে"),
            ["UrgentThenRecentOrder"] = ("Urgent first, newest order date", "জরুরি আগে, সবচেয়ে সাম্প্রতিক অর্ডারের তারিখ আগে"),
            ["OrderDateFrom"] = ("Order Date From", "তারিখ থেকে অর্ডার"),
            ["OrderDateTo"] = ("Order Date To", "তারিখ পর্যন্ত অর্ডার"),
            ["DeliveryDateFrom"] = ("Delivery Date From", "তারিখ থেকে ডেলিভারি"),
            ["DeliveryDateTo"] = ("Delivery Date To", "তারিখ পর্যন্ত ডেলিভারি"),
            ["LoadingOrders"] = ("Loading orders...", "অর্ডার লোড হচ্ছে..."),
            ["AdjustFilters"] = ("Try adjusting your filters or search keywords.", "ফিল্টার বা অনুসন্ধানের শব্দ পরিবর্তন করে দেখুন।"),
            ["Paid"] = ("Paid", "পেইড"),
            ["Balance"] = ("Balance", "ব্যালেনস"),
            ["CategoryAndDescription"] = ("Category", "ক্যাটাগরি"),
            ["FabricDetails"] = ("Fabric Details", "কাপড়ের তথ্য"),
            ["SheilaSize"] = ("Sheila Size", "শেলার মাপ"),
            ["CurrentState"] = ("Current State", "বর্তমান অবস্থা"),
            ["Showing"] = ("Showing", "দেখানো হচ্ছে"),
            ["Of"] = ("of", "মোট"),
            ["WorkflowHistory"] = ("Workflow History", "কাজের ধাপের ইতিহাস"),
            ["Time"] = ("Time", "সময়"),
            ["Event"] = ("Event", "ঘটনা"),
            ["Transition"] = ("Transition", "পরিবর্তন"),
            ["Notes"] = ("Notes", "নোট"),
            ["LoadingWorkflowHistory"] = ("Loading workflow history...", "কাজের ধাপের ইতিহাস লোড হচ্ছে..."),
            ["NoWorkflowEventsRecorded"] = ("No workflow events were recorded for this item.", "এই পোশাকের কোনো কাজের ধাপের তথ্য পাওয়া যায়নি।"),
            ["ErrorLoadingData"] = ("Error Loading Data", "তথ্য লোড করতে সমস্যা হয়েছে"),
            ["Error"] = ("Error", "সমস্যা"),
            ["InitialState"] = ("Initial state", "প্রাথমিক অবস্থা"),
            ["ReadyItemsDescription"] = ("Receive completed items from the workshop and deliver showroom-ready items.", "ওয়ার্কশপ থেকে সম্পন্ন পোশাক গ্রহণ করুন এবং শোরুমে প্রস্তুত পোশাক ডেলিভারি দিন।"),
            ["ReadyItemsCount"] = ("Ready Items", "প্রস্তুত পোশাক"),
            ["Workshop"] = ("Workshop", "ওয়ার্কশপ"),
            ["Delivery"] = ("Delivery", "ডেলিভারি"),
            ["Deliver"] = ("Deliver", "ডেলিভারি দিন"),
            ["DeliveryPayment"] = ("Delivery payment", "ডেলিভারির পেমেন্ট"),
            ["OrderNoLabel"] = ("Order No", "অর্ডার নম্বর"),
            ["Location"] = ("Location", "অবস্থান"),
            ["WorkshopAndShowroom"] = ("Workshop and Showroom", "ওয়ার্কশপ ও শোরুম"),
            ["ReadyAtShowroom"] = ("Ready at Showroom", "শোরুমে প্রস্তুত"),
            ["NoReadyItems"] = ("No Ready Items Found", "কোনো প্রস্তুত পোশাক পাওয়া যায়নি"),
            ["NoItemsMatchFilters"] = ("No items match the selected filters.", "নির্বাচিত ফিল্টারে কোনো পোশাক পাওয়া যায়নি।"),
            ["AllItemsReady"] = ("All items ready", "সব পোশাক প্রস্তুত"),
            ["ItemsRemaining"] = ("item(s) remaining", "টি পোশাক বাকি"),
            ["ReceiveWorkshopItems"] = ("Receive Workshop Items", "ওয়ার্কশপের পোশাক গ্রহণ করুন"),
            ["ReceiveAtShop"] = ("Receive at Shop", "শোরুমে গ্রহণ করুন"),
            ["DeliverSelected"] = ("Deliver Selected", "নির্বাচিত পোশাক ডেলিভারি দিন"),
            ["SelectReadyItems"] = ("Select ready items for delivery", "ডেলিভারির জন্য প্রস্তুত পোশাক নির্বাচন করুন"),
            ["ItemSelection"] = ("Select", "নির্বাচন"),
            ["DescriptionLabel"] = ("Description", "বিবরণ"),
            ["ActualDelivery"] = ("Actual Delivery", "প্রকৃত ডেলিভারি"),
            ["PaymentSummary"] = ("Payment Summary", "পেমেন্টের সারসংক্ষেপ"),
            ["OrderTotal"] = ("Order Total", "অর্ডারের মোট"),
            ["PaidToDate"] = ("Paid to Date", "এ পর্যন্ত পরিশোধিত"),
            ["CurrentPayment"] = ("Current Payment", "বর্তমান পেমেন্ট"),
            ["BalanceAfterPayment"] = ("Balance After Payment", "পেমেন্টের পর বকেয়া"),
            ["PaymentHistory"] = ("Payment History", "পেমেন্টের ইতিহাস"),
            ["PaymentDate"] = ("Payment Date", "পেমেন্টের তারিখ"),
            ["PaymentAmount"] = ("Payment Amount", "পেমেন্টের পরিমাণ"),
            ["InitialDeposit"] = ("Initial Deposit", "প্রাথমিক অগ্রিম"),
            ["NoPaymentHistory"] = ("No payment history found.", "কোনো পেমেন্টের ইতিহাস পাওয়া যায়নি।"),
            ["ConfirmDeliveryPayment"] = ("Confirm Delivery & Payment", "ডেলিভারি ও পেমেন্ট নিশ্চিত করুন"),
            ["PaymentRequired"] = ("Enter a payment amount before delivering selected items.", "নির্বাচিত পোশাক ডেলিভারি দেওয়ার আগে পেমেন্টের পরিমাণ লিখুন।"),
            ["PaymentCannotExceedBalance"] = ("Payment cannot exceed the current balance due.", "পেমেন্ট বর্তমান বকেয়ার চেয়ে বেশি হতে পারবে না।"),
            ["ReceiveItemQuestion"] = ("Receive Item at Showroom?", "পোশাকটি শোরুমে গ্রহণ করবেন?"),
            ["ReceiveItemText"] = ("Move this item from the workshop to the showroom?", "এই পোশাকটি ওয়ার্কশপ থেকে শোরুমে আনবেন?"),
            ["ReceiveWorkshopQuestion"] = ("Receive Workshop Items?", "ওয়ার্কশপের পোশাক গ্রহণ করবেন?"),
            ["ReceiveWorkshopText"] = ("Receive the workshop items for this order at the showroom?", "এই অর্ডারের ওয়ার্কশপের পোশাক শোরুমে গ্রহণ করবেন?"),
            ["YesReceive"] = ("Yes, Receive", "হ্যাঁ, গ্রহণ করুন"),
            ["YesReceiveItems"] = ("Yes, Receive Items", "হ্যাঁ, পোশাক গ্রহণ করুন"),
            ["YesDeliver"] = ("Yes, Deliver", "হ্যাঁ, ডেলিভারি দিন"),
            ["CancelDelivery"] = ("Cancel", "বাতিল"),
            ["Unknown"] = ("Unknown", "অজানা"),
            ["SelectWorkflow"] = ("-- Select Workflow / Tailor --", "-- কাজের ধাপ / দর্জি নির্বাচন করুন --"),
            ["ValidationError"] = ("Validation Error", "তথ্য যাচাইয়ের সমস্যা"),
            ["SelectWorkflowRequired"] = ("Please select a workflow / tailor option.", "কাজের ধাপ / টেইলার নির্বাচন করুন।"),
            ["WorkflowStatusChange"] = ("Workflow Status Change", "কাজের ধাপের পরিবর্তন"),
            ["ExternalVendorDispatchEvent"] = ("External Vendor Dispatch", "বাইরে কাজ পাঠানো"),
            ["ExternalVendorReturnEvent"] = ("External Vendor Return", "বাইরের কাজ রিসিভ "),
            ["TrackTailorTasks"] = ("Track assigned garment stitching tasks, execute active work, and advance items in the workflow.", "নিয়োগ করা পোশাকের সেলাইয়ের কাজ দেখুন, চলমান কাজ সম্পন্ন করুন এবং পোশাককে পরবর্তী ধাপে পাঠান।"),
            ["CurrentlyWorkingTask"] = ("Currently Working Task", "বর্তমানে চলমান কাজ"),
            ["ViewMeasurements"] = ("View Measurements", "মাপ দেখুন"),
            ["CompleteStitching"] = ("Complete Stitching", "সেলাই সম্পন্ন করুন"),
            ["LoadingTailorTasks"] = ("Loading tailor tasks...", "দর্জির কাজ লোড হচ্ছে..."),
            ["NoAssignedTasks"] = ("No Assigned Tasks Found", "কোনো কাজ পাওয়া যায়নি"),
            ["NoAssignedTasksDescription"] = ("There are no pending stitching tasks assigned for the selected filters.", "নির্বাচিত ফিল্টারে কোনো সেলাইয়ের কাজ পেন্ডিং নেই।"),
            ["OrderNoHeader"] = ("ORDER NO", "অর্ডার নং"),
            ["CategoryAndModel"] = ("Category & Model", "ধরন ও মডেল"),
            ["FabricAndColor"] = ("Fabric & Color", "কাপড় ও কালার"),
            ["Flags"] = ("Flags", "বৈশিষ্ট্য"),
            ["Start"] = ("Start", "শুরু করুন"),
            ["Active"] = ("Active", "চলমান"),
            ["NA"] = ("N/A", "প্রযোজ্য নয়"),
            ["Shop"] = ("Shop", "দোকান"),
            ["Inches"] = ("in", "ইঞ্চি"),
            ["Sheila"] = ("Sheila", "শীলা"),
            ["ButtonsCount"] = ("buttons", "বোতামের সংখ্যা"),
            ["ActiveTaskInProgress"] = ("Active Task In Progress", "একটি কাজ বর্তমানে চলমান"),
            ["ActiveTaskWarning"] = ("You already have an active stitching task. Complete it before starting a new one.", "আপনার একটি সেলাইয়ের কাজ ইতিমধ্যে চলছে। নতুন কাজ শুরু করার আগে এটি সম্পন্ন করুন।"),
            ["StartStitchingTaskQuestion"] = ("Start Stitching Task?", "সেলাইয়ের কাজ শুরু করবেন?"),
            ["StartTaskText"] = ("Start working on this order and item?", "এই অর্ডার ও পোশাকের কাজ শুরু করবেন?"),
            ["YesStartTask"] = ("Yes, Start Task", "হ্যাঁ, কাজ শুরু করুন"),
            ["TaskStarted"] = ("Task Started", "কাজ শুরু হয়েছে"),
            ["ItemNowActive"] = ("Item is now active.", "কাজ এখন চলছে।"),
            ["CompleteStitchingTaskQuestion"] = ("Complete Stitching Task?", "সেলাইয়ের কাজ সম্পন্ন হয়েছে?"),
            ["CompleteTaskText"] = ("Mark stitching for this item as complete and move to the next stage?", "এই পোশাকের সেলাই কমপ্লিট হিসেবে চিহ্নিত করে পরবর্তী ধাপে পাঠাবেন?"),
            ["YesCompleteTask"] = ("Yes, Complete Task", "হ্যাঁ, কাজ সম্পন্ন করুন"),
            ["TaskCompleted"] = ("Task Completed!", "কাজ কমপ্লিট হয়েছে!"),
            ["ItemMovedToNextStage"] = ("The item was moved to the next stage.", "আইটেমটি পরবর্তী ধাপে পাঠানো হয়েছে।"),
            ["TailorStartedStitching"] = ("Tailor started the stitching task.", "টেইলার সেলাইয়ের কাজ শুরু করেছেন।"),
            ["TailorCompletedStitching"] = ("Tailor completed the stitching task.", "টেইলার সেলাইয়ের কাজ কমপ্লিট করেছেন।"),
            ["OrderDates"] = ("Order dates", "অর্ডারের তারিখ"),
            ["To"] = ("to", "থেকে"),
            ["TotalOrders"] = ("Total Orders", "মোট অর্ডার"),
            ["ActiveItems"] = ("Active Items", "চলমান আইটেম"),
            ["DeliveredItems"] = ("Delivered Items", "ডেলিভারি হওয়া পোশাক"),
            ["OverdueOrders"] = ("Overdue Orders", "লেট হওয়া অর্ডার"),
            ["ProductionPipeline"] = ("Production Pipeline", "উৎপাদনের অগ্রগতি"),
            ["ProductionPipelineDescription"] = ("Current item distribution by workflow status", "কাজের ধাপ অনুযায়ী আইটেমের বর্তমান বণ্টন"),
            ["FinancialOverview"] = ("Financial Overview", "আর্থিক সারসংক্ষেপ"),
            ["SelectedScopeTotals"] = ("Totals for the selected scope", "নির্বাচিত পরিসরের মোট হিসাব"),
            ["DepositsReceived"] = ("Deposits Received", "প্রাপ্ত অগ্রিম"),
            ["ExternalWorkInProgress"] = ("External Work in Progress", "চলমান বাইরের কাজ"),
            ["WorkersCurrentTasks"] = ("Workers' Current Tasks", "কারিগরদের বর্তমান কাজ"),
            ["WorkerTasksDescription"] = ("Active internal worker assignments and task start times", "চলমান অভ্যন্তরীণ কর্মী নিয়োগ ও কাজ শুরুর সময়"),
            ["NoActiveWorkerTasks"] = ("No active worker tasks found.", "কোনো চলমান কারিগরের কাজ পাওয়া যায়নি।"),
            ["Worker"] = ("Worker", "কারিগর"),
            ["WorkerPerformance"] = ("Worker Performance", "কারিগরের কর্মদক্ষতা"),
            ["WorkerPerformanceDescription"] = ("Review an individual worker's activity, completed work, and workflow history.", "একজন কারিগরের কাজ, সম্পন্ন কাজ ও কাজের ধাপের ইতিহাস দেখুন।"),
            ["SelectWorker"] = ("Select Worker", "কারিগর নির্বাচন করুন"),
            ["AllWorkers"] = ("Select a worker", "একজন কারিগর নির্বাচন করুন"),
            ["FromDate"] = ("From date", "শুরুর তারিখ"),
            ["ToDate"] = ("To date", "শেষ তারিখ"),
            ["LoadReport"] = ("Load Report", "রিপোর্ট দেখুন"),
            ["TotalWorkflowEvents"] = ("Workflow Events", "কাজের ধাপের ঘটনা"),
            ["CompletedItems"] = ("Completed Items", "সম্পন্ন পোশাক"),
            ["ActiveItems"] = ("Active Items", "চলমান পোশাক"),
            ["LastActivity"] = ("Last Activity", "সর্বশেষ কাজ"),
            ["NoWorkerSelected"] = ("Select a worker to view performance.", "কর্মদক্ষতা দেখতে একজন কারিগর নির্বাচন করুন।"),
            ["NoWorkerHistory"] = ("No workflow history found for this worker and date range.", "এই কারিগর ও তারিখের জন্য কোনো কাজের ধাপের ইতিহাস পাওয়া যায়নি।"),
            ["WorkerName"] = ("Worker", "কারিগর"),
            ["Role"] = ("Role", "দায়িত্ব"),
            ["CurrentTask"] = ("Current Task", "বর্তমান কাজ"),
            ["StartedAt"] = ("Started At", "শুরুর সময়"),
            ["RecentOrders"] = ("Recent Orders", "সাম্প্রতিক অর্ডার"),
            ["RecentOrdersDescription"] = ("The latest orders in the selected scope", "নির্বাচিত পরিসরের সর্বশেষ অর্ডার"),
            ["UndeliveredItems"] = ("Undelivered Items", "ডেলিভারি না হওয়া আইটেম"),
            ["UndeliveredItemsDescription"] = ("All outstanding items, sorted by delivery date", "ডেলিভারির তারিখ অনুযায়ী সাজানো বাকি আইটেম"),
            ["Amount"] = ("Amount", "এমাউন্ট"),
            ["Urgent"] = ("Urgent", "আর্জেন্ট"),
            ["AllOrders"] = ("All orders", "সব অর্ডার"),
            ["ExternalVendorTracking"] = ("External Vendor Tracking", "বাইরের কারিগরের কাজ ট্রাকিং"),
            ["ReadyItems"] = ("Ready Items", "প্রস্তুত পোশাক"),
            ["DataEntry"] = ("Data Entry", "তথ্য সংযোজন"),
            ["InternalWorkerEntry"] = ("Internal Worker Entry", "ভেতরের কর্মী তথ্য"),
            ["ExternalWorkerEntry"] = ("External Worker Entry", "বাইরের কারিগরের তথ্য"),
            ["FabricSupplierEntry"] = ("Fabric Supplier Entry", "কাপড়ের দোকানের তথ্য"),
            ["FabricTypesEntry"] = ("Fabric Types Entry", "কাপড়ের ধরনের তথ্য"),
            ["ReadymadeSheila"] = ("Readymade Sheila", "রেডিমেড শেলা"),
            ["ReadymadeSales"] = ("Readymade Sales", "রেডিমেড সেল"),
            ["Notifications"] = ("Workflow notifications", "কাজের ধাপের বিজ্ঞপ্তি"),
            ["ExitPortal"] = ("Exit Portal", "বের হয়ে যান"),
            ["SignIn"] = ("Sign In", "প্রবেশ করুন"),
            ["Close"] = ("Close", "বন্ধ করুন"),
            ["Language"] = ("Language", "ভাষা"),
            ["English"] = ("English", "ইংরেজি"),
            ["Bengali"] = ("বাংলা", "বাংলা"),
            ["New"] = ("New", "নতুন"),
            ["JustNow"] = ("Just now", "এইমাত্র"),
            ["MinuteAgo"] = ("minute", "মিনিট"),
            ["HourAgo"] = ("hour", "ঘণ্টা"),
            ["DayAgo"] = ("day", "দিন"),
            ["MonthAgo"] = ("month", "মাস"),
            ["YearAgo"] = ("year", "বছর"),
            ["ReadyForFabricProcurement"] = ("Ready for Fabric Procurement", "কাপড় কেনার জন্য প্রস্তুত"),
            ["QueueRawFabricEmb"] = ("Queued for Raw Fabric Embroidery", "থান কাপড়ের এম্ব্রয়ডারির অপেক্ষায়"),
            ["OutForRawFabricEmb"] = ("Out for Raw Fabric Embroidery", "থান কাপড়ের এম্ব্রয়ডারির জন্য বাইরে"),
            ["QueueCut"] = ("Queued for Cutting", "কাটিংয়ের অপেক্ষায়"),
            ["QueueHalfStitching"] = ("Queued for Half Stitching", "আধা সেলাইয়ের অপেক্ষায়"),
            ["HalfStitchActive"] = ("Half Stitching Active", "আধা সেলাই চলছে"),
            ["QueueHalfStitchEmb"] = ("Queued for Half-Stitch Embroidery", "আধা সেলাইয়ের হাতের কাজের অপেক্ষায়"),
            ["OutForHalfStitchEmb"] = ("Out for Half-Stitch Embroidery", "আধা সেলাইয়ের হাতের কাজের জন্য বাইরে"),
            ["QueueHandEmbAssignment"] = ("Waiting for Hand Embroidery Assignment", "হাতের কাজে দেয়ার অপেক্ষায়"),
            ["QueueHandEmb"] = ("Queued for Hand Embroidery", "হাতের কাজের অপেক্ষায়"),
            ["HandEmbActive"] = ("Hand Embroidery Active", "হাতের কাজ চলছে"),
            ["QueueFullStitching"] = ("Queued for Full Stitching", "পুরো সেলাইয়ের অপেক্ষায়"),
            ["FullStitchActive"] = ("Full Stitching Active", "পুরো সেলাই চলছে"),
            ["QueueExternalVendor"] = ("Queued for External Vendor", "বাইরে পাঠানোর অপেক্ষায়"),
            ["OutWithExternalVendor"] = ("Out with External Vendor", "বাইরের কারিগরের কাছে আছে"),
            ["ReadyAtWorkShop"] = ("Ready at Workshop", "ওয়ার্কশপে রেডি"),
            ["ReadyAtShop"] = ("Ready at Shop", "শোরুমে রেডি"),
            ["Delivered"] = ("Delivered", "ডেলিভারি হয়েছে"),
            ["Internal"] = ("Internal Workshop", "কারখানা"),
            ["Hybrid"] = ("Hybrid", "বাইরের কাজ আধা"),
            ["External"] = ("External", "বাইরের কাজ পুরা"),
            ["Both"] = ("Both (Hybrid & Full)", "বাইরে আধা ও পুরা কাজ")
            ,
            ["EditOrder"] = ("Edit Order", "অর্ডার এডিট ")
            ,
            ["LoadingOrderInformation"] = ("Loading order information...", "অর্ডারের তথ্য লোড হচ্ছে...")
            ,
            ["OrderInfo"] = ("Order Info", "অর্ডারের তথ্য")
            ,
            ["LineItems"] = ("Line Items", "আইটেমের লিস্ট")
            ,
            ["Measurements"] = ("Measurements", "মাপ")
            ,
            ["PaymentReview"] = ("Payment & Review", "পেমেন্ট ও যাচাই")
            ,
            ["MasterOrderInformation"] = ("Master Order Information", "মূল অর্ডারের তথ্য")
            ,
            ["UrgentOrder"] = ("Urgent Order", "আরজেন্ট অর্ডার")
            ,
            ["Replenishment"] = ("Replenishment", "দোকানের জন্য")
            ,
            ["ShowroomBranch"] = ("Showroom Branch", "শোরুম শাখা")
            ,
            ["SelectBranch"] = ("-- Select Branch --", "-- শাখা নির্বাচন করুন --")
            ,
            ["OrderNo"] = ("Order No", "অর্ডার নম্বর")
            ,
            ["OrderDate"] = ("Order Date", "অর্ডারের তারিখ")
            ,
            ["CustomerName"] = ("Customer Name", "কাস্টমারের নাম")
            ,
            ["CustomerPhone"] = ("Customer Phone", "কাস্টমারের ফোন")
            ,
            ["NextStep"] = ("Next Step", "পরের ধাপ")
            ,
            ["PreviousStep"] = ("Previous Step", "আগের ধাপ")
            ,
            ["SavingOrder"] = ("Saving Order...", "অর্ডার সেইভ হচ্ছে...")
            ,
            ["UpdateOrder"] = ("Update Order", "অর্ডার আপডেট করুন")
            ,
            ["SaveEntireOrder"] = ("Save Entire Order", "সম্পূর্ণ অর্ডার সেইভ করুন")
            ,
            ["MostRecentWorkflowEvents"] = ("Most recent workflow events", "সাম্প্রতিক কাজের ধাপের তথ্য")
            ,
            ["CloseNotifications"] = ("Close notifications", "বিজ্ঞপ্তি বন্ধ করুন")
            ,
            ["LoadingEvents"] = ("Loading events...", "তথ্য লোড হচ্ছে...")
            ,
            ["NoWorkflowEvents"] = ("No workflow events found.", "কাজের ধাপের কোনো তথ্য পাওয়া যায়নি।")
            ,
            ["OrderLabel"] = ("Order", "অর্ডার")
            ,
            ["ItemLabel"] = ("Item", "পোশাক")
            ,
            ["CustomerUnavailable"] = ("Customer unavailable", "কাস্টমারের তথ্য নেই")
            ,
            ["Responsible"] = ("Responsible", "কর্মীর নাম")
            ,
            ["AccessDenied"] = ("You do not have permission to access this page.", "এই পেজ দেখার অনুমতি আপনার নেই।")
            ,
            ["LogIn"] = ("Log In", "লগইন")
            ,
            ["LoadingSecurityProfile"] = ("Loading security profile...", "নিরাপত্তা তথ্য লোড হচ্ছে...")
            ,
            ["FabricProcurementTitle"] = ("Fabric Procurement", "কাপড় কেনা")
            ,
            ["CuttingTitle"] = ("Cutting Dashboard", "কাটিং ড্যাশবোর্ড")
            ,
            ["TailorAssignmentTitle"] = ("Tailor Assignment", "টেইলারকে কাজ দেয়া")
            ,
            ["TailorDashboardTitle"] = ("Tailors Dashboard", "টেইলার ড্যাশবোর্ড")
            ,
            ["ExternalDispatchTitle"] = ("External Vendor Dispatch", "বাইরের কাজ পাঠানো")
            ,
            ["ExternalReturnsTitle"] = ("External Vendor Returns", "বাইরের কাজ রিসিভ করা")
            ,
            ["HandEmbAssignmentTitle"] = ("Hand Embroidery Assignment", "হাতের কাজ দেয়া")
            ,
            ["HandEmbDashboardTitle"] = ("Hand Embroiderer Dashboard", "হাতের কাজের ড্যাশবোর্ড")
            ,
            ["ExternalTrackingTitle"] = ("External Vendor Tracking", "বাইরের কারিগরের কাজ দেখা")
            ,
            ["ReadyItemsTitle"] = ("Ready Items", "রেডি আইটেম")
            ,
            ["Pending"] = ("Pending", "পেন্ডিং")
            ,
            ["Action"] = ("Action", "কাজ")
            ,
            ["OrderID"] = ("Order ID", "অর্ডার আইডি")
            ,
            ["ItemID"] = ("Item ID", "আইটেম আইডি")
            ,
            ["FabricName"] = ("Fabric Name", "কাপড়ের নাম")
            ,
            ["FabricShopName"] = ("Fabric Shop Name", "কাপড়ের দোকানের নাম")
            ,
            ["ColorCode"] = ("Color Code", "কালার কোড")
            ,
            ["AllFabricShops"] = ("All Fabric Shops", "সব কাপড়ের দোকান")
            ,
            ["AllVendors"] = ("All Vendors", "সব বাইরের কারিগর")
            ,
            ["ExpectedReturn"] = ("Expected Return", "রিসিভের সম্ভাব্য তারিখ")
            ,
            ["DeliveryDate"] = ("Delivery Date", "ডেলিভারির তারিখ")
            ,
            ["OrderNoTicket"] = ("Order No / Ticket", "অর্ডার নং")
            ,
            ["SearchOrderNumber"] = ("Search order number", "অর্ডার নম্বর খুঁজুন")
            ,
            ["Loading"] = ("Loading...", "লোড হচ্ছে...")
            ,
            ["Customer"] = ("Customer", "কাস্টমার")
            ,
            ["WorkflowStage"] = ("Workflow Stage", "কাজের ধাপ")
            ,
            ["AssignedVendor"] = ("Assigned Vendor", "বাইরের কারিগর")
            ,
            ["AssignedTailor"] = ("Assigned Tailor", "টেইলার")
            ,
            ["Unassigned"] = ("Unassigned", "কাজ দেয়া হয়নি")
            ,
            ["Cancel"] = ("Cancel", "বাতিল")
            ,
            ["Confirm"] = ("Confirm", "কনফার্ম")
            ,
            ["Search"] = ("Search", "খুঁজুন")
            ,
            ["Reset"] = ("Reset", "রিসেট")
            ,
            ["AllBranches"] = ("All Branches", "সব শাখা")
            ,
            ["Branch"] = ("Branch", "শাখা")
            ,
            ["Order"] = ("Order", "অর্ডার")
            ,
            ["Status"] = ("Status", "অবস্থা")
            ,
            ["Date"] = ("Date", "তারিখ")
            ,
            ["Name"] = ("Name", "নাম")
            ,
            ["Phone"] = ("Phone", "ফোন নং")
            ,
            ["Save"] = ("Save", "সেইভ")
            ,
            ["Update"] = ("Update", "আপডেট")
            ,
            ["Delete"] = ("Delete", "ডিলেট")
            ,
            ["Add"] = ("Add", "এড করুন")
            ,
            ["SelectTailor"] = ("-- Select tailor --", "-- টেইলার সিলেক্ট করুন --")
            ,
            ["AllTailors"] = ("-- All Tailors --", "-- সব টেইলার --")
            ,
            ["AllItemsDelivered"] = ("All items have been delivered.", "সব আইটেম ডেলিভারি হয়েছে।")
            ,
            ["NoOrdersFound"] = ("No orders found.", "কোনো অর্ডার পাওয়া যায়নি।")
            ,
            ["NoPendingItems"] = ("No pending items found.", "কোনো পেন্ডিং আইটেম পাওয়া যায়নি।")
            ,
            ["ReadymadeSalesTitle"] = ("Readymade Sales", "রেডিমেড সেল")
            ,
            ["ReadymadeSheilaTitle"] = ("Readymade Sheila", "রেডিমেড শেলা")
            ,
            ["All"] = ("All", "সব")
            ,
            ["RawFabricEmbroidery"] = ("Raw fabric embroidery", "থান কাপড়ের এম্ব্রয়ডারি")
            ,
            ["HalfStitchEmbroidery"] = ("Half-stitch embroidery", "আধা এম্ব্রয়ডারি")
            ,
            ["FullExternalProduction"] = ("Full external production", "পুরা বাইরের কাজ")
            ,
            ["ItemCategory"] = ("Item Category", "পোশাকের ধরন")
            ,
            ["Abaya"] = ("Abaya", "আবায়া")
            ,
            ["InnerDress"] = ("Inner Dress", "ইনার ড্রেস")
            ,
            ["ModelDescription"] = ("Model Description", "মডেলের বিবরণ")
            ,
            ["WorkflowTailor"] = ("Workflow / Assigned Tailor", "কাজের ধাপ / টেইলার")
            ,
            ["SelectTailorOption"] = ("-- Select Tailor --", "-- টেইলার সিলেক্ট করুন --")
            ,
            ["InternalWorkshop"] = ("Internal Workshop", "কারখানা")
            ,
            ["FullExternal"] = ("Full External", "পুরা বাইরের কাজ")
            ,
            ["BuyExternalFabric"] = ("Buy fabric for this external worker model?", "কাপড় দোকান থেকে দিবেন?")
            ,
            ["FabricSupplierShop"] = ("Fabric Supplier Shop", "কাপড়ের দোকান")
            ,
            ["SelectShop"] = ("-- Select Shop --", "-- দোকান সিলেক্ট করুন --")
            ,
            ["FabricType"] = ("Fabric Type", "কাপড়ের ধরন")
            ,
            ["SelectFabric"] = ("-- Select Fabric --", "-- কাপড় সিলেক্ট করুন --")
            ,
            ["CatalogColorCode"] = ("Catalog Color Code", "ক্যাটালগের কালার কোড")
            ,
            ["StandardSize"] = ("Standard (28 x 81 in)", "স্ট্যান্ডার্ড (২৮ x ৮১ ইঞ্চি)")
            ,
            ["SmallSize"] = ("Small (22 x 81 in)", "ছোট (২২ x ৮১ ইঞ্চি)")
            ,
            ["CustomXL"] = ("Large (28 x 90 in)", "বড় (২৮ x ৯০ ইঞ্চি)")
            ,
            ["HandEmbRequired"] = ("Hand Embroidery Required?", "হাতের কাজ আছে?")
            ,
            ["RawFabricExternal"] = ("Send Raw Uncut Fabric to External Embroiderer?", "থান কাপড় বাইরের কারিগরের কাছে পাঠাবেন?")
            ,
            ["EditItem"] = ("Edit Item", " এডিট আইটেম")
            ,
            ["AddNewItem"] = ("Add New Item to Order", "নতুন আইটেম এড করুন")
            ,
            ["EditingMode"] = ("Editing Mode Active", "এডিট মোড চালু")
            ,
            ["UpdateLineItem"] = ("Update Line Item", "আইটেম তথ্য আপডেট করুন")
            ,
            ["AddItem"] = ("Add Item to Order List", "অর্ডারের তালিকায় আইটেম যোগ করুন")
            ,
            ["Category"] = ("Category", "ধরন")
            ,
            ["Description"] = ("Description", "বিবরণ")
            ,
            ["Color"] = ("Color", "কালার")
            ,
            ["EmbroideryOptions"] = ("Embroidery Options", "এম্ব্রয়ডারি/হাতের কাজের অপশন")
            ,
            ["Actions"] = ("Actions", "কাজসমূহ")
            ,
            ["None"] = ("None", "নেই")
            ,
            ["Locked"] = ("Locked", "লক করা")
            ,
            ["MeasurementsSkipped"] = ("Measurements are skipped for replenishment orders.", "দোকানের আবায়ার জন্য মাপের তথ্য লাগবে না।")
            ,
            ["CustomerMeasurements"] = ("Customer Measurements Profile", "কাস্টমারের মাপ")
            ,
            ["OptionalSkippable"] = ("Optional / Skippable", "অপশনাল")
            ,
            ["ButtonConfiguration"] = ("Button Configuration", "বোতামের ধরন")
            ,
            ["NoButtons"] = ("No Buttons", "বোতাম নেই")
            ,
            ["ButtonsWithBand"] = ("Buttons With Band", "পট্টিসহ বোতাম")
            ,
            ["ButtonsWithoutBand"] = ("Buttons Without Band", "পট্টি ছাড়া বোতাম")
            ,
            ["NumberOfButtons"] = ("Number of Buttons", "বোতামের সংখ্যা")
            ,
            ["FinancialSummary"] = ("Financial Calculation & Delivery Summary", "আর্থিক হিসাব ও ডেলিভারির সারসংক্ষেপ")
            ,
            ["OrderType"] = ("Order Type", "অর্ডারের ধরন")
            ,
            ["OrderTicketNo"] = ("Order Ticket No", "অর্ডার টিকিট নম্বর")
            ,
            ["TotalLineItems"] = ("Total Line Items", "মোট পোশাক")
            ,
            ["Items"] = ("Items", "পোশাক")
            ,
            ["EstimatedDelivery"] = ("Estimated Delivery Date", "সম্ভাব্য ডেলিভারির তারিখ")
            ,
            ["TotalAmount"] = ("Total Amount", "মোট টাকা")
            ,
            ["DepositPaid"] = ("Deposit Paid", "এডভান্স দিয়েছে")
            ,
            ["BalanceDue"] = ("Balance Due", "বাকি টাকা")
            ,
            ["OrderNotes"] = ("Order Notes / Instructions", "অর্ডারের নোট / নির্দেশনা")
            ,
            ["CustomerDetailsNotRequired"] = ("Customer details are not required for replenishment orders.", "দোকানের আবায়ার জন্য কাস্টমারের তথ্য প্রয়োজন নেই।")
            ,
            ["NoCustomerMeasurements"] = ("No customer measurements are available for this item.", "এই আইটেমের জন্য কাস্টমারের মাপের তথ্য পাওয়া যায়নি।")
            ,
            ["CustomerMeasurementsTitle"] = ("Customer Measurements", "কাস্টমারের মাপ")
            ,
            ["CuttingDescription"] = ("Manage orders pending fabric cutting and assign completed cut garments to tailors.", "কাপড় কাটার অপেক্ষায় থাকা অর্ডার পরিচালনা করুন এবং কাটা আইটেম দর্জিদের দিন।")
            ,
            ["PendingCut"] = ("Pending Cut", "কাটিং বাকি")
            ,
            ["Filter"] = ("Filter", "ফিল্টার")
            ,
            ["SearchOrderNo"] = ("Search order no...", "অর্ডার নম্বর খুঁজুন...")
            ,
            ["LoadingCuttingQueue"] = ("Loading cutting queue orders...", "কাটিংয়ের অর্ডার লোড হচ্ছে...")
            ,
            ["NoPendingCutItems"] = ("No Pending Items in Cutting Queue", "কাটিংয়ের অপেক্ষায় কোনো আইটেম নেই")
            ,
            ["CutTasksProcessed"] = ("All cut tasks have been processed or no orders match your filter criteria.", "সব কাটিংয়ের কাজ সম্পন্ন হয়েছে অথবা ফিল্টারে কোনো অর্ডার মেলেনি।")
            ,
            ["OrderTicket"] = ("Order Ticket", "অর্ডার নং")
            ,
            ["Garment"] = ("Garment", "পোশাক")
            ,
            ["Fabric"] = ("Fabric", "কাপড়")
            ,
            ["NextTargetStage"] = ("Next Target Stage", "পরবর্তী কাজের ধাপ")
            ,
            ["QueueFullStitch"] = ("Queue Full-Stitching", "সম্পূর্ণ সেলাইয়ের অপেক্ষায়")
            ,
            ["SelectAvailableTailor"] = ("Select Available Tailor", "এভেইলেবল টেইলার সিলেক্ট করুন")
            ,
            ["Username"] = ("Username", "ইউজারনেম")
            ,
            ["Assigned"] = ("assigned", "এসাইন করা")
            ,
            ["SelectAssign"] = ("Select & Assign", "সিলেক্ট করে এসাইন করুন")
            ,
            ["MarkCutOnly"] = ("Mark as Cut Only", "শুধু কাটা হয়েছে হিসেবে মার্ক করুন")
            ,
            ["ViewSpecs"] = ("View Specs", "মাপ দেখুন")
            ,
            ["HandEmb"] = ("Hand Emb", "হাতের কাজ")
            ,
            ["RawFabricEmb"] = ("Raw Fabric Emb", "থান কাপড়ের এম্ব্রয়ডারি")
            ,
            ["StandardStitching"] = ("Standard Stitching", "সাধারণ সেলাই")
            ,
            ["Garments"] = ("Garment(s)", "পোশাক")
            ,
            ["CutAndAssignTailor"] = ("Cut & Assign Tailor", "কেটে টেইলারের কাছে দিন")
            ,
            ["Due"] = ("Due", "ডেলিভারির তারিখ")
            ,
            ["Size_22x81"] = ("Small (22 x 81)", "ছোট (২২ x ৮১)")
            ,
            ["Size_28x81"] = ("Standard (28 x 81)", "স্ট্যান্ডার্ড (২৮ x ৮১)")
            ,
            ["Size_28x90"] = ("Large (28 x 90)", "বড় (২৮ x ৯০)")
            ,
            ["AssignTailorForItem"] = ("Assign Tailor for Item", "টেইলারকে কাজ দিন")
            ,
            ["RequiresEmbroidery"] = ("Requires Embroidery", "হাতের কাজ আছে")
            ,
            ["NoRegisteredTailors"] = ("No registered Tailors found in the system. Please add tailors under Worker Management.", "সিস্টেমে কোনো টেইলার পাওয়া যায়নি। কর্মী ব্যবস্থাপনা থেকে টেইলার এড করুন।")
            ,
            ["MarkItemCut"] = ("Mark as Cut", "কাটা হয়েছে হিসেবে মার্ক করুন")
            ,
            ["ConfirmTailorAssignment"] = ("Confirm Tailor Assignment?", "টেইলারকে কাজ দিচ্ছেন কনফার্ম?")
            ,
            ["MarkItemCutQuestion"] = ("Mark item as cut without assigning a tailor?", "টেইলারকে কাজ না দিয়ে পোশাকটি কাটা হয়েছে হিসেবে মার্ক করবেন?")
            ,
            ["YesCutAssign"] = ("Yes, Cut & Assign", "হ্যাঁ, কাটিং করে কাজ বুঝিয়ে দিন")
            ,
            ["YesMarkCut"] = ("Yes, Mark as Cut", "হ্যাঁ, কাটা হয়েছে হিসেবে মার্ক করুন")
            ,
            ["ItemCutAssigned"] = ("Item cut and assigned successfully.", "কাপড় কেটে সফলভাবে টেইলারের কাছে দেওয়া হয়েছে।")
            ,
            ["ItemWaitingTailor"] = ("Item is waiting for tailor assignment.", "আইটেমটি টেইলারকে বুঝিয়ে দেয়ার অপেক্ষায় আছে।")
            ,
            ["ConfirmCutAssignText"] = ("Mark the item as cut and assign it to this tailor?", "আইটেমটি কাটা হয়েছে হিসেবে মার্ক করে এই টেইলারের কাছে দেবেন?")
            ,
            ["AssignedToTailorDashboard"] = ("The item has been assigned to the tailor's dashboard.", "আইটেমটি টেইলারের ড্যাশবোর্ডে দেওয়া হয়েছে।")
            ,
            ["ItemCutWithoutTailor"] = ("Mark the item as cut without assigning a tailor?", "টেইলার নিয়োগ না করে আইটেমটি কাটা হয়েছে হিসেবে চিহ্নিত করবেন?")
            ,
            ["AbayaFrontLength"] = ("Abaya Length (Front)", "আবায়ার লম্বা (সামনে)")
            ,
            ["AbayaBackLength"] = ("Abaya Length (Back)", "আবায়ার লম্বা (পেছনে)")
            ,
            ["SleeveLength"] = ("Sleeve Length", "আস্তিনের লম্বা")
            ,
            ["ArmHoleWidth"] = ("Arm Hole Width", "মোরা")
            ,
            ["SleeveOpeningWidth"] = ("Sleeve Opening Width", "আস্তিনের মুখ")
            ,
            ["ShoulderWidth"] = ("Shoulder Width", "কাঁধ")
            ,
            ["BodyChestWidth"] = ("Body / Chest Width", "বডি")
            ,
            ["BottomFlareWidth"] = ("Bottom / Flare Width", "কোলোশ")
            ,
            ["WorkflowCreated"] = ("Initial workflow status assigned when the order was created.", "অর্ডার তৈরি করার সময় কাজের ধাপ নির্ধারণ করা হয়েছে।")
            ,
            ["WorkflowUpdated"] = ("Workflow status assigned while the order was updated.", "অর্ডার আপডেট করার সময় কাজের ধাপ নির্ধারণ করা হয়েছে।")
            ,
            ["FabricProcurementCompleted"] = ("Fabric procurement completed.", "কাপড় কেনা হয়েছে।")
            ,
            ["AbayaFabricProcured"] = ("Abaya fabric procured.", "আবায়ার কাপড় কেনা হয়েছে।")
            ,
            ["TailorAssignedAfterCutting"] = ("Tailor assigned after cutting was completed.", "কাটিং শেষ হওয়ার পর টেইলারকে কাজ দেয়া হয়েছে।")
            ,
            ["TailorAssignedAtCutting"] = ("Tailor assigned at the time of cutting.", "কাটিংয়ের সময় টেইলারকে কাজ দেয়া হয়েছে।")
            ,
            ["CuttingCompleted"] = ("Cutting completed.", "কাটিং করা হয়েছে।")
            ,
            ["HandEmbAssigned"] = ("Hand embroiderer assigned.", "হাতের কাজের কারিগরকে দেয়া হয়েছে।")
            ,
            ["HandEmbStarted"] = ("Hand embroiderer started the task.", "হাতের কাজের কারিগর কাজ শুরু করেছেন।")
            ,
            ["HandEmbCompleted"] = ("Hand embroidery completed.", "হাতের কাজ সম্পন্ন হয়েছে।")
            ,
            ["ItemReceivedAtShowroom"] = ("Item received at showroom.", "আইটেম শোরুমে রিসিভ করা হয়েছে।")
            ,
            ["ItemDelivered"] = ("Item delivered to customer.", "আইটেম কাস্টমারকে ডেলিভারি করা হয়েছে।")
            ,
            ["TranslateWorkflowNoteFallback"] = ("", "")
        };

    public bool IsBengali { get; private set; }
    public int Version { get; private set; }
    public event Action? Changed;

    public string this[string key] => _texts.TryGetValue(key, out var value)
        ? (IsBengali ? value.Bengali : value.English)
        : key;

    public string ForStatus(ItemStatus? status) => status.HasValue
        ? this[status.Value.ToString()]
        : this["New"];

    public string TranslateWorkflowNote(string? note)
    {
        if (string.IsNullOrWhiteSpace(note)) return string.Empty;

        return note switch
        {
            "WorkflowCreated" => this["WorkflowCreated"],
            "WorkflowUpdated" => this["WorkflowUpdated"],
            "FabricProcurementCompleted" => this["FabricProcurementCompleted"],
            "AbayaFabricProcured" => this["AbayaFabricProcured"],
            "TailorAssignedAfterCutting" => this["TailorAssignedAfterCutting"],
            "TailorAssignedAtCutting" => this["TailorAssignedAtCutting"],
            "CuttingCompleted" => this["CuttingCompleted"],
            "HandEmbAssigned" => this["HandEmbAssigned"],
            "HandEmbStarted" => this["HandEmbStarted"],
            "HandEmbCompleted" => this["HandEmbCompleted"],
            "ItemReceivedAtShowroom" => this["ItemReceivedAtShowroom"],
            "ItemDelivered" => this["ItemDelivered"],
            "DeliveryPayment" => this["DeliveryPayment"],
            "TailorStartedStitching" => this["TailorStartedStitching"],
            "TailorCompletedStitching" => this["TailorCompletedStitching"],
            "Initial workflow status assigned when the order was created." => this["WorkflowCreated"],
            "Workflow status assigned while the order was updated." => this["WorkflowUpdated"],
            "Fabric procurement completed." => this["FabricProcurementCompleted"],
            "Abaya fabric procured." => this["AbayaFabricProcured"],
            "Tailor assigned after cutting was completed." => this["TailorAssignedAfterCutting"],
            "Tailor assigned at the time of cutting." => this["TailorAssignedAtCutting"],
            "Cutting completed." => this["CuttingCompleted"],
            "Hand embroiderer assigned." => this["HandEmbAssigned"],
            "Hand embroiderer started the task." => this["HandEmbStarted"],
            "Hand embroidery completed." => this["HandEmbCompleted"],
            "Item received at showroom." => this["ItemReceivedAtShowroom"],
            "Item delivered to customer." => this["ItemDelivered"],
            "Tailor started the stitching task." => this["TailorStartedStitching"],
            "Tailor completed the stitching task." => this["TailorCompletedStitching"],
            "Delivery payment" => this["DeliveryPayment"],
            _ => note
        };
    }

    public string TranslateEventType(string? eventType) => eventType switch
    {
        "Workflow Status Change" => this["WorkflowStatusChange"],
        "External Vendor Dispatch" => this["ExternalVendorDispatchEvent"],
        "External Vendor Return" => this["ExternalVendorReturnEvent"],
        _ => eventType ?? string.Empty
    };

    public void SetLanguage(bool bengali)
    {
        if (IsBengali == bengali) return;
        IsBengali = bengali;
        Version++;
        Changed?.Invoke();
    }
}
