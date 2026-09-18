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
            ["OrderIntake"] = ("Order Intake", "অর্ডার গ্রহণ"),
            ["FabricProcurement"] = ("Fabric Procurement", "কাপড় ক্রয়"),
            ["CuttingDashboard"] = ("Cutting Dashboard", "কাটিং ড্যাশবোর্ড"),
            ["TailorsAssignment"] = ("Tailors Assignment", "দর্জি নিয়োগ"),
            ["TailorsDashboard"] = ("Tailors Dashboard", "দর্জি ড্যাশবোর্ড"),
            ["ExternalDispatch"] = ("External Dispatch", "বাইরের কারিগরের কাছে পাঠানো"),
            ["ExternalVendorReturns"] = ("External Vendor Returns", "বাইরের কারিগরের ফেরত"),
            ["HandEmbroideryAssignment"] = ("Hand Embroidery Assignment", "হাতের কাজের নিয়োগ"),
            ["HandEmbroidererDashboard"] = ("Hand Embroiderer Dashboard", "হাতের কাজের ড্যাশবোর্ড"),
            ["Overview"] = ("Overview", "সারসংক্ষেপ"),
            ["Dashboard"] = ("Dashboard", "ড্যাশবোর্ড"),
            ["DashboardDescription"] = ("A live overview of orders, production, delivery, and collections", "অর্ডার, উৎপাদন, ডেলিভারি ও আদায়ের সামগ্রিক চিত্র"),
            ["AssignedItems"] = ("Assigned Items", "নিয়োগ করা পোশাক"),
            ["OrdersDescription"] = ("Search, filter, track status, and manage all customer orders", "সব ক্রেতার অর্ডার খুঁজুন, ফিল্টার করুন, অবস্থা দেখুন ও পরিচালনা করুন"),
            ["CreateNewOrder"] = ("Create New Order", "নতুন অর্ডার তৈরি করুন"),
            ["FilterSearchOptions"] = ("Filter & Search Options", "ফিল্টার ও অনুসন্ধানের বিকল্প"),
            ["ResetFilters"] = ("Reset Filters", "ফিল্টার রিসেট করুন"),
            ["SearchTicket"] = ("Search Ticket #", "টিকিট নম্বর খুঁজুন"),
            ["SearchCustomer"] = ("Search Customer...", "ক্রেতা খুঁজুন..."),
            ["SearchPhone"] = ("Search Phone...", "ফোন নম্বর খুঁজুন..."),
            ["AllStates"] = ("All States", "সব অবস্থা"),
            ["SortByUrgentFirst"] = ("Sort By (Urgent First)", "সাজান (জরুরি আগে)"),
            ["OrderDateFrom"] = ("Order Date From", "অর্ডারের তারিখ থেকে"),
            ["OrderDateTo"] = ("Order Date To", "অর্ডারের তারিখ পর্যন্ত"),
            ["DeliveryDateFrom"] = ("Delivery Date From", "ডেলিভারির তারিখ থেকে"),
            ["DeliveryDateTo"] = ("Delivery Date To", "ডেলিভারির তারিখ পর্যন্ত"),
            ["LoadingOrders"] = ("Loading orders...", "অর্ডার লোড হচ্ছে..."),
            ["AdjustFilters"] = ("Try adjusting your filters or search keywords.", "ফিল্টার বা অনুসন্ধানের শব্দ পরিবর্তন করে দেখুন।"),
            ["Paid"] = ("Paid", "পরিশোধিত"),
            ["Balance"] = ("Balance", "বকেয়া"),
            ["CategoryAndDescription"] = ("Category", "ধরন"),
            ["FabricDetails"] = ("Fabric Details", "কাপড়ের তথ্য"),
            ["SheilaSize"] = ("Sheila Size", "শীলার মাপ"),
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
            ["Unknown"] = ("Unknown", "অজানা"),
            ["SelectWorkflow"] = ("-- Select Workflow / Tailor --", "-- কাজের ধাপ / দর্জি নির্বাচন করুন --"),
            ["ValidationError"] = ("Validation Error", "তথ্য যাচাইয়ের সমস্যা"),
            ["SelectWorkflowRequired"] = ("Please select a workflow / tailor option.", "কাজের ধাপ / দর্জির বিকল্প নির্বাচন করুন।"),
            ["WorkflowStatusChange"] = ("Workflow Status Change", "কাজের ধাপের পরিবর্তন"),
            ["ExternalVendorDispatchEvent"] = ("External Vendor Dispatch", "বাইরের কারিগরের কাছে পাঠানো"),
            ["ExternalVendorReturnEvent"] = ("External Vendor Return", "বাইরের কারিগরের কাছ থেকে ফেরত"),
            ["TrackTailorTasks"] = ("Track assigned garment stitching tasks, execute active work, and advance items in the workflow.", "নিয়োগ করা পোশাকের সেলাইয়ের কাজ দেখুন, চলমান কাজ সম্পন্ন করুন এবং পোশাককে পরবর্তী ধাপে পাঠান।"),
            ["CurrentlyWorkingTask"] = ("Currently Working Task", "বর্তমানে চলমান কাজ"),
            ["ViewMeasurements"] = ("View Measurements", "মাপ দেখুন"),
            ["CompleteStitching"] = ("Complete Stitching", "সেলাই সম্পন্ন করুন"),
            ["LoadingTailorTasks"] = ("Loading tailor tasks...", "দর্জির কাজ লোড হচ্ছে..."),
            ["NoAssignedTasks"] = ("No Assigned Tasks Found", "কোনো নিয়োগ করা কাজ পাওয়া যায়নি"),
            ["NoAssignedTasksDescription"] = ("There are no pending stitching tasks assigned for the selected filters.", "নির্বাচিত ফিল্টারে কোনো অপেক্ষমাণ সেলাইয়ের কাজ নেই।"),
            ["OrderNoHeader"] = ("ORDER NO", "অর্ডার নম্বর"),
            ["CategoryAndModel"] = ("Category & Model", "ধরন ও মডেল"),
            ["FabricAndColor"] = ("Fabric & Color", "কাপড় ও রঙ"),
            ["Flags"] = ("Flags", "বৈশিষ্ট্য"),
            ["Start"] = ("Start", "শুরু করুন"),
            ["Active"] = ("Active", "চলমান"),
            ["NA"] = ("N/A", "প্রযোজ্য নয়"),
            ["Shop"] = ("Shop", "দোকান"),
            ["Inches"] = ("in", "ইঞ্চি"),
            ["Sheila"] = ("Sheila", "শীলা"),
            ["ButtonsCount"] = ("buttons", "টি বোতাম"),
            ["ActiveTaskInProgress"] = ("Active Task In Progress", "একটি কাজ বর্তমানে চলমান"),
            ["ActiveTaskWarning"] = ("You already have an active stitching task. Complete it before starting a new one.", "আপনার একটি সেলাইয়ের কাজ ইতিমধ্যে চলছে। নতুন কাজ শুরু করার আগে এটি সম্পন্ন করুন।"),
            ["StartStitchingTaskQuestion"] = ("Start Stitching Task?", "সেলাইয়ের কাজ শুরু করবেন?"),
            ["StartTaskText"] = ("Start working on this order and item?", "এই অর্ডার ও পোশাকের কাজ শুরু করবেন?"),
            ["YesStartTask"] = ("Yes, Start Task", "হ্যাঁ, কাজ শুরু করুন"),
            ["TaskStarted"] = ("Task Started", "কাজ শুরু হয়েছে"),
            ["ItemNowActive"] = ("Item is now active.", "পোশাকের কাজ এখন চলছে।"),
            ["CompleteStitchingTaskQuestion"] = ("Complete Stitching Task?", "সেলাইয়ের কাজ সম্পন্ন করবেন?"),
            ["CompleteTaskText"] = ("Mark stitching for this item as complete and move to the next stage?", "এই পোশাকের সেলাই সম্পন্ন হিসেবে চিহ্নিত করে পরবর্তী ধাপে পাঠাবেন?"),
            ["YesCompleteTask"] = ("Yes, Complete Task", "হ্যাঁ, কাজ সম্পন্ন করুন"),
            ["TaskCompleted"] = ("Task Completed!", "কাজ সম্পন্ন হয়েছে!"),
            ["ItemMovedToNextStage"] = ("The item was moved to the next stage.", "পোশাকটি পরবর্তী ধাপে পাঠানো হয়েছে।"),
            ["TailorStartedStitching"] = ("Tailor started the stitching task.", "দর্জি সেলাইয়ের কাজ শুরু করেছেন।"),
            ["TailorCompletedStitching"] = ("Tailor completed the stitching task.", "দর্জি সেলাইয়ের কাজ সম্পন্ন করেছেন।"),
            ["OrderDates"] = ("Order dates", "অর্ডারের তারিখ"),
            ["To"] = ("to", "থেকে"),
            ["TotalOrders"] = ("Total Orders", "মোট অর্ডার"),
            ["ActiveItems"] = ("Active Items", "চলমান পোশাক"),
            ["DeliveredItems"] = ("Delivered Items", "ডেলিভারি হওয়া পোশাক"),
            ["OverdueOrders"] = ("Overdue Orders", "বিলম্বিত অর্ডার"),
            ["ProductionPipeline"] = ("Production Pipeline", "উৎপাদনের অগ্রগতি"),
            ["ProductionPipelineDescription"] = ("Current item distribution by workflow status", "কাজের ধাপ অনুযায়ী পোশাকের বর্তমান বণ্টন"),
            ["FinancialOverview"] = ("Financial Overview", "আর্থিক সারসংক্ষেপ"),
            ["SelectedScopeTotals"] = ("Totals for the selected scope", "নির্বাচিত পরিসরের মোট হিসাব"),
            ["DepositsReceived"] = ("Deposits Received", "প্রাপ্ত অগ্রিম"),
            ["ExternalWorkInProgress"] = ("External Work in Progress", "চলমান বাইরের কাজ"),
            ["WorkersCurrentTasks"] = ("Workers' Current Tasks", "কর্মীদের বর্তমান কাজ"),
            ["WorkerTasksDescription"] = ("Active internal worker assignments and task start times", "চলমান অভ্যন্তরীণ কর্মী নিয়োগ ও কাজ শুরুর সময়"),
            ["NoActiveWorkerTasks"] = ("No active worker tasks found.", "কোনো চলমান কর্মীর কাজ পাওয়া যায়নি।"),
            ["Worker"] = ("Worker", "কর্মী"),
            ["Role"] = ("Role", "দায়িত্ব"),
            ["CurrentTask"] = ("Current Task", "বর্তমান কাজ"),
            ["StartedAt"] = ("Started At", "শুরুর সময়"),
            ["RecentOrders"] = ("Recent Orders", "সাম্প্রতিক অর্ডার"),
            ["RecentOrdersDescription"] = ("The latest orders in the selected scope", "নির্বাচিত পরিসরের সর্বশেষ অর্ডার"),
            ["UndeliveredItems"] = ("Undelivered Items", "ডেলিভারি না হওয়া পোশাক"),
            ["UndeliveredItemsDescription"] = ("All outstanding items, sorted by delivery date", "ডেলিভারির তারিখ অনুযায়ী সাজানো বাকি পোশাক"),
            ["Amount"] = ("Amount", "অর্থ"),
            ["Urgent"] = ("Urgent", "জরুরি"),
            ["AllOrders"] = ("All orders", "সব অর্ডার"),
            ["ExternalVendorTracking"] = ("External Vendor Tracking", "বাইরের কারিগরের কাজ পর্যবেক্ষণ"),
            ["ReadyItems"] = ("Ready Items", "প্রস্তুত পোশাক"),
            ["DataEntry"] = ("Data Entry", "তথ্য সংযোজন"),
            ["InternalWorkerEntry"] = ("Internal Worker Entry", "ভেতরের কর্মী তথ্য"),
            ["ExternalWorkerEntry"] = ("External Worker Entry", "বাইরের কর্মী তথ্য"),
            ["FabricSupplierEntry"] = ("Fabric Supplier Entry", "কাপড় সরবরাহকারী তথ্য"),
            ["FabricTypesEntry"] = ("Fabric Types Entry", "কাপড়ের ধরন তথ্য"),
            ["ReadymadeSheila"] = ("Readymade Sheila", "রেডিমেড শীলা"),
            ["ReadymadeSales"] = ("Readymade Sales", "রেডিমেড বিক্রয়"),
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
            ["QueueRawFabricEmb"] = ("Queued for Raw Fabric Embroidery", "কাঁচা কাপড়ের হাতের কাজের অপেক্ষায়"),
            ["OutForRawFabricEmb"] = ("Out for Raw Fabric Embroidery", "কাঁচা কাপড়ের হাতের কাজের জন্য বাইরে"),
            ["QueueCut"] = ("Queued for Cutting", "কাটিংয়ের অপেক্ষায়"),
            ["QueueHalfStitching"] = ("Queued for Half Stitching", "অর্ধেক সেলাইয়ের অপেক্ষায়"),
            ["HalfStitchActive"] = ("Half Stitching Active", "অর্ধেক সেলাই চলছে"),
            ["QueueHalfStitchEmb"] = ("Queued for Half-Stitch Embroidery", "অর্ধেক সেলাইয়ের হাতের কাজের অপেক্ষায়"),
            ["OutForHalfStitchEmb"] = ("Out for Half-Stitch Embroidery", "অর্ধেক সেলাইয়ের হাতের কাজের জন্য বাইরে"),
            ["QueueHandEmbAssignment"] = ("Waiting for Hand Embroidery Assignment", "হাতের কাজের কর্মী নিয়োগের অপেক্ষায়"),
            ["QueueHandEmb"] = ("Queued for Hand Embroidery", "হাতের কাজের অপেক্ষায়"),
            ["HandEmbActive"] = ("Hand Embroidery Active", "হাতের কাজ চলছে"),
            ["QueueFullStitching"] = ("Queued for Full Stitching", "সম্পূর্ণ সেলাইয়ের অপেক্ষায়"),
            ["FullStitchActive"] = ("Full Stitching Active", "সম্পূর্ণ সেলাই চলছে"),
            ["QueueExternalVendor"] = ("Queued for External Vendor", "বাইরের কারিগরের কাছে পাঠানোর অপেক্ষায়"),
            ["OutWithExternalVendor"] = ("Out with External Vendor", "বাইরের কারিগরের কাছে আছে"),
            ["ReadyAtWorkShop"] = ("Ready at Workshop", "ওয়ার্কশপে প্রস্তুত"),
            ["ReadyAtShop"] = ("Ready at Shop", "শোরুমে প্রস্তুত"),
            ["Delivered"] = ("Delivered", "ডেলিভারি হয়েছে"),
            ["Internal"] = ("Internal Workshop", "নিজস্ব ওয়ার্কশপ"),
            ["Hybrid"] = ("Hybrid", "মিশ্র কাজ"),
            ["External"] = ("External", "বাইরের কারিগর"),
            ["Both"] = ("Both (Hybrid & Full)", "দুই ধরনের কাজ")
            , ["EditOrder"] = ("Edit Order", "অর্ডার সম্পাদনা")
            , ["LoadingOrderInformation"] = ("Loading order information...", "অর্ডারের তথ্য লোড হচ্ছে...")
            , ["OrderInfo"] = ("Order Info", "অর্ডারের তথ্য")
            , ["LineItems"] = ("Line Items", "পোশাকের তালিকা")
            , ["Measurements"] = ("Measurements", "মাপ")
            , ["PaymentReview"] = ("Payment & Review", "পেমেন্ট ও যাচাই")
            , ["MasterOrderInformation"] = ("Master Order Information", "মূল অর্ডারের তথ্য")
            , ["UrgentOrder"] = ("Urgent Order", "জরুরি অর্ডার")
            , ["Replenishment"] = ("Replenishment", "স্টক পূরণ")
            , ["ShowroomBranch"] = ("Showroom Branch", "শোরুম শাখা")
            , ["SelectBranch"] = ("-- Select Branch --", "-- শাখা নির্বাচন করুন --")
            , ["OrderNo"] = ("Order No", "অর্ডার নম্বর")
            , ["OrderDate"] = ("Order Date", "অর্ডারের তারিখ")
            , ["CustomerName"] = ("Customer Name", "ক্রেতার নাম")
            , ["CustomerPhone"] = ("Customer Phone", "ক্রেতার ফোন")
            , ["NextStep"] = ("Next Step", "পরের ধাপ")
            , ["PreviousStep"] = ("Previous Step", "আগের ধাপ")
            , ["SavingOrder"] = ("Saving Order...", "অর্ডার সংরক্ষণ হচ্ছে...")
            , ["UpdateOrder"] = ("Update Order", "অর্ডার আপডেট করুন")
            , ["SaveEntireOrder"] = ("Save Entire Order", "সম্পূর্ণ অর্ডার সংরক্ষণ করুন")
            , ["MostRecentWorkflowEvents"] = ("Most recent workflow events", "সাম্প্রতিক কাজের ধাপের তথ্য")
            , ["CloseNotifications"] = ("Close notifications", "বিজ্ঞপ্তি বন্ধ করুন")
            , ["LoadingEvents"] = ("Loading events...", "তথ্য লোড হচ্ছে...")
            , ["NoWorkflowEvents"] = ("No workflow events found.", "কাজের ধাপের কোনো তথ্য পাওয়া যায়নি।")
            , ["OrderLabel"] = ("Order", "অর্ডার")
            , ["ItemLabel"] = ("Item", "পোশাক")
            , ["CustomerUnavailable"] = ("Customer unavailable", "ক্রেতার তথ্য নেই")
            , ["Responsible"] = ("Responsible", "দায়িত্বপ্রাপ্ত")
            , ["AccessDenied"] = ("You do not have permission to access this page.", "এই পেজ দেখার অনুমতি আপনার নেই।")
            , ["LogIn"] = ("Log In", "লগইন")
            , ["LoadingSecurityProfile"] = ("Loading security profile...", "নিরাপত্তা তথ্য লোড হচ্ছে...")
            , ["FabricProcurementTitle"] = ("Fabric Procurement", "কাপড় ক্রয়")
            , ["CuttingTitle"] = ("Cutting Dashboard", "কাটিং ড্যাশবোর্ড")
            , ["TailorAssignmentTitle"] = ("Tailor Assignment", "দর্জি নিয়োগ")
            , ["TailorDashboardTitle"] = ("Tailors Dashboard", "দর্জি ড্যাশবোর্ড")
            , ["ExternalDispatchTitle"] = ("External Vendor Dispatch", "বাইরের কারিগরের কাছে পাঠানো")
            , ["ExternalReturnsTitle"] = ("External Vendor Returns", "বাইরের কারিগরের ফেরত")
            , ["HandEmbAssignmentTitle"] = ("Hand Embroidery Assignment", "হাতের কাজের নিয়োগ")
            , ["HandEmbDashboardTitle"] = ("Hand Embroiderer Dashboard", "হাতের কাজের ড্যাশবোর্ড")
            , ["ExternalTrackingTitle"] = ("External Vendor Tracking", "বাইরের কারিগরের কাজ পর্যবেক্ষণ")
            , ["ReadyItemsTitle"] = ("Ready Items", "প্রস্তুত পোশাক")
            , ["Pending"] = ("Pending", "অপেক্ষমাণ")
            , ["Action"] = ("Action", "কাজ")
            , ["OrderID"] = ("Order ID", "অর্ডার আইডি")
            , ["ItemID"] = ("Item ID", "পোশাক আইডি")
            , ["FabricName"] = ("Fabric Name", "কাপড়ের নাম")
            , ["FabricShopName"] = ("Fabric Shop Name", "কাপড়ের দোকানের নাম")
            , ["ColorCode"] = ("Color Code", "রঙের কোড")
            , ["AllFabricShops"] = ("All Fabric Shops", "সব কাপড়ের দোকান")
            , ["AllVendors"] = ("All Vendors", "সব বাইরের কারিগর")
            , ["ExpectedReturn"] = ("Expected Return", "ফেরতের সম্ভাব্য তারিখ")
            , ["DeliveryDate"] = ("Delivery Date", "ডেলিভারির তারিখ")
            , ["OrderNoTicket"] = ("Order No / Ticket", "অর্ডার নম্বর / টিকিট")
            , ["SearchOrderNumber"] = ("Search order number", "অর্ডার নম্বর খুঁজুন")
            , ["Loading"] = ("Loading...", "লোড হচ্ছে...")
            , ["Customer"] = ("Customer", "ক্রেতা")
            , ["WorkflowStage"] = ("Workflow Stage", "কাজের ধাপ")
            , ["AssignedVendor"] = ("Assigned Vendor", "নিয়োগ করা বাইরের কারিগর")
            , ["AssignedTailor"] = ("Assigned Tailor", "নিয়োগ করা দর্জি")
            , ["Unassigned"] = ("Unassigned", "নিয়োগ করা হয়নি")
            , ["Cancel"] = ("Cancel", "বাতিল")
            , ["Confirm"] = ("Confirm", "নিশ্চিত করুন")
            , ["Search"] = ("Search", "খুঁজুন")
            , ["Reset"] = ("Reset", "রিসেট")
            , ["AllBranches"] = ("All Branches", "সব শাখা")
            , ["Branch"] = ("Branch", "শাখা")
            , ["Order"] = ("Order", "অর্ডার")
            , ["Status"] = ("Status", "অবস্থা")
            , ["Date"] = ("Date", "তারিখ")
            , ["Name"] = ("Name", "নাম")
            , ["Phone"] = ("Phone", "ফোন")
            , ["Save"] = ("Save", "সংরক্ষণ")
            , ["Update"] = ("Update", "আপডেট")
            , ["Delete"] = ("Delete", "মুছে ফেলুন")
            , ["Add"] = ("Add", "যোগ করুন")
            , ["SelectTailor"] = ("-- Select tailor --", "-- দর্জি নির্বাচন করুন --")
            , ["AllTailors"] = ("-- All Tailors --", "-- সব দর্জি --")
            , ["AllItemsDelivered"] = ("All items have been delivered.", "সব পোশাক ডেলিভারি হয়েছে।")
            , ["NoOrdersFound"] = ("No orders found.", "কোনো অর্ডার পাওয়া যায়নি।")
            , ["NoPendingItems"] = ("No pending items found.", "কোনো অপেক্ষমাণ পোশাক পাওয়া যায়নি।")
            , ["ReadymadeSalesTitle"] = ("Readymade Sales", "রেডিমেড বিক্রয়")
            , ["ReadymadeSheilaTitle"] = ("Readymade Sheila", "রেডিমেড শীলা")
            , ["All"] = ("All", "সব")
            , ["RawFabricEmbroidery"] = ("Raw fabric embroidery", "কাঁচা কাপড়ের হাতের কাজ")
            , ["HalfStitchEmbroidery"] = ("Half-stitch embroidery", "অর্ধেক সেলাইয়ের হাতের কাজ")
            , ["FullExternalProduction"] = ("Full external production", "সম্পূর্ণ বাইরের কাজ")
            , ["ItemCategory"] = ("Item Category", "পোশাকের ধরন")
            , ["Abaya"] = ("Abaya", "আবায়া")
            , ["InnerDress"] = ("Inner Dress (Internal Only)", "ইনার ড্রেস (শুধু নিজস্ব কাজ)")
            , ["ModelDescription"] = ("Model / Style Description", "মডেল / ডিজাইনের বিবরণ")
            , ["WorkflowTailor"] = ("Workflow / Assigned Tailor", "কাজের ধাপ / নিয়োগ করা দর্জি")
            , ["SelectTailorOption"] = ("-- Select Tailor --", "-- দর্জি নির্বাচন করুন --")
            , ["InternalWorkshop"] = ("Internal Workshop", "নিজস্ব ওয়ার্কশপ")
            , ["FullExternal"] = ("Full External", "সম্পূর্ণ বাইরের কাজ")
            , ["BuyExternalFabric"] = ("Buy fabric for this external worker model?", "এই বাইরের কারিগরের মডেলের জন্য কাপড় কিনবেন?")
            , ["FabricSupplierShop"] = ("Fabric Supplier Shop", "কাপড় সরবরাহকারী দোকান")
            , ["SelectShop"] = ("-- Select Shop --", "-- দোকান নির্বাচন করুন --")
            , ["FabricType"] = ("Fabric Type", "কাপড়ের ধরন")
            , ["SelectFabric"] = ("-- Select Fabric --", "-- কাপড় নির্বাচন করুন --")
            , ["CatalogColorCode"] = ("Catalog Color Code", "ক্যাটালগের রঙের কোড")
            , ["StandardSize"] = ("Standard (28 x 81 in)", "সাধারণ (২৮ x ৮১ ইঞ্চি)")
            , ["SmallSize"] = ("Small (22 x 81 in)", "ছোট (২২ x ৮১ ইঞ্চি)")
            , ["CustomXL"] = ("Custom XL (28 x 90 in - Needs Fabric)", "কাস্টম XL (২৮ x ৯০ ইঞ্চি - কাপড় প্রয়োজন)")
            , ["HandEmbRequired"] = ("Hand Embroidery Required?", "হাতের কাজ প্রয়োজন?")
            , ["RawFabricExternal"] = ("Send Raw Uncut Fabric to External Embroiderer?", "কাঁচা কাপড় বাইরের হাতের কাজের কারিগরের কাছে পাঠাবেন?")
            , ["EditItem"] = ("Edit Item", "পোশাক সম্পাদনা")
            , ["AddNewItem"] = ("Add New Item to Order", "অর্ডারে নতুন পোশাক যোগ করুন")
            , ["EditingMode"] = ("Editing Mode Active", "সম্পাদনার মোড চালু")
            , ["UpdateLineItem"] = ("Update Line Item", "পোশাকের তথ্য আপডেট করুন")
            , ["AddItem"] = ("Add Item to Order List", "অর্ডারের তালিকায় পোশাক যোগ করুন")
            , ["Category"] = ("Category", "ধরন")
            , ["Description"] = ("Description", "বিবরণ")
            , ["Color"] = ("Color", "রঙ")
            , ["EmbroideryOptions"] = ("Embroidery Options", "হাতের কাজের বিকল্প")
            , ["Actions"] = ("Actions", "কাজসমূহ")
            , ["None"] = ("None", "নেই")
            , ["Locked"] = ("Locked", "লক করা")
            , ["MeasurementsSkipped"] = ("Measurements are skipped for replenishment orders.", "স্টক পূরণের অর্ডারে মাপের তথ্য লাগবে না।")
            , ["CustomerMeasurements"] = ("Customer Measurements Profile", "ক্রেতার মাপের তথ্য")
            , ["OptionalSkippable"] = ("Optional / Skippable", "ঐচ্ছিক / বাদ দেওয়া যায়")
            , ["ButtonConfiguration"] = ("Button Configuration", "বোতামের ধরন")
            , ["NoButtons"] = ("No Buttons", "বোতাম নেই")
            , ["ButtonsWithBand"] = ("Buttons With Band", "ব্যান্ডসহ বোতাম")
            , ["ButtonsWithoutBand"] = ("Buttons Without Band", "ব্যান্ড ছাড়া বোতাম")
            , ["NumberOfButtons"] = ("Number of Buttons", "বোতামের সংখ্যা")
            , ["FinancialSummary"] = ("Financial Calculation & Delivery Summary", "আর্থিক হিসাব ও ডেলিভারির সারসংক্ষেপ")
            , ["OrderType"] = ("Order Type", "অর্ডারের ধরন")
            , ["OrderTicketNo"] = ("Order Ticket No", "অর্ডার টিকিট নম্বর")
            , ["TotalLineItems"] = ("Total Line Items", "মোট পোশাক")
            , ["Items"] = ("Items", "পোশাক")
            , ["EstimatedDelivery"] = ("Estimated Delivery Date", "সম্ভাব্য ডেলিভারির তারিখ")
            , ["TotalAmount"] = ("Total Amount", "মোট টাকা")
            , ["DepositPaid"] = ("Deposit Paid", "জমা দেওয়া টাকা")
            , ["BalanceDue"] = ("Balance Due", "বাকি টাকা")
            , ["OrderNotes"] = ("Order Notes / Instructions", "অর্ডারের নোট / নির্দেশনা")
            , ["CustomerDetailsNotRequired"] = ("Customer details are not required for replenishment orders.", "স্টক পূরণের অর্ডারে ক্রেতার তথ্য প্রয়োজন নেই।")
            , ["NoCustomerMeasurements"] = ("No customer measurements are available for this item.", "এই পোশাকের জন্য ক্রেতার মাপের তথ্য পাওয়া যায়নি।")
            , ["CustomerMeasurementsTitle"] = ("Customer Measurements", "ক্রেতার মাপ")
            , ["CuttingDescription"] = ("Manage orders pending fabric cutting and assign completed cut garments to tailors.", "কাপড় কাটার অপেক্ষায় থাকা অর্ডার পরিচালনা করুন এবং কাটা পোশাক দর্জিদের দিন।")
            , ["PendingCut"] = ("Pending Cut", "কাটিং বাকি")
            , ["Filter"] = ("Filter", "ফিল্টার")
            , ["SearchOrderNo"] = ("Search order no...", "অর্ডার নম্বর খুঁজুন...")
            , ["LoadingCuttingQueue"] = ("Loading cutting queue orders...", "কাটিংয়ের অর্ডার লোড হচ্ছে...")
            , ["NoPendingCutItems"] = ("No Pending Items in Cutting Queue", "কাটিংয়ের অপেক্ষায় কোনো পোশাক নেই")
            , ["CutTasksProcessed"] = ("All cut tasks have been processed or no orders match your filter criteria.", "সব কাটিংয়ের কাজ সম্পন্ন হয়েছে অথবা ফিল্টারে কোনো অর্ডার মেলেনি।")
            , ["OrderTicket"] = ("Order Ticket", "অর্ডার টিকিট")
            , ["Garment"] = ("Garment", "পোশাক")
            , ["Fabric"] = ("Fabric", "কাপড়")
            , ["NextTargetStage"] = ("Next Target Stage", "পরবর্তী কাজের ধাপ")
            , ["QueueFullStitch"] = ("Queue Full-Stitching", "সম্পূর্ণ সেলাইয়ের অপেক্ষায়")
            , ["SelectAvailableTailor"] = ("Select Available Tailor", "উপলব্ধ দর্জি নির্বাচন করুন")
            , ["Username"] = ("Username", "ইউজারনেম")
            , ["Assigned"] = ("assigned", "নিয়োগ করা")
            , ["SelectAssign"] = ("Select & Assign", "নির্বাচন করে নিয়োগ করুন")
            , ["MarkCutOnly"] = ("Mark as Cut Only", "শুধু কাটা হয়েছে হিসেবে চিহ্নিত করুন")
            , ["ViewSpecs"] = ("View Specs", "মাপ দেখুন")
            , ["HandEmb"] = ("Hand Emb", "হাতের কাজ")
            , ["RawFabricEmb"] = ("Raw Fabric Emb", "কাঁচা কাপড়ের হাতের কাজ")
            , ["StandardStitching"] = ("Standard Stitching", "সাধারণ সেলাই")
            , ["Garments"] = ("Garment(s)", "পোশাক")
            , ["CutAndAssignTailor"] = ("Cut & Assign Tailor", "কেটে দর্জির কাছে দিন")
            , ["Due"] = ("Due", "ডেলিভারির তারিখ")
            , ["Size_22x81"] = ("Small (22 x 81)", "ছোট (২২ x ৮১)")
            , ["Size_28x81"] = ("Standard (28 x 81)", "সাধারণ (২৮ x ৮১)")
            , ["Size_28x90"] = ("Large (28 x 90)", "বড় (২৮ x ৯০)")
            , ["AssignTailorForItem"] = ("Assign Tailor for Item", "পোশাকের জন্য দর্জি নিয়োগ")
            , ["RequiresEmbroidery"] = ("Requires Embroidery", "হাতের কাজ প্রয়োজন")
            , ["NoRegisteredTailors"] = ("No registered Tailors found in the system. Please add tailors under Worker Management.", "সিস্টেমে কোনো দর্জি পাওয়া যায়নি। কর্মী ব্যবস্থাপনা থেকে দর্জি যোগ করুন।")
            , ["MarkItemCut"] = ("Mark as Cut", "কাটা হয়েছে হিসেবে চিহ্নিত করুন")
            , ["ConfirmTailorAssignment"] = ("Confirm Tailor Assignment?", "দর্জি নিয়োগ নিশ্চিত করবেন?")
            , ["MarkItemCutQuestion"] = ("Mark item as cut without assigning a tailor?", "দর্জি নিয়োগ না করে পোশাকটি কাটা হয়েছে হিসেবে চিহ্নিত করবেন?")
            , ["YesCutAssign"] = ("Yes, Cut & Assign", "হ্যাঁ, কাটিং ও নিয়োগ করুন")
            , ["YesMarkCut"] = ("Yes, Mark as Cut", "হ্যাঁ, কাটা হয়েছে হিসেবে চিহ্নিত করুন")
            , ["ItemCutAssigned"] = ("Item cut and assigned successfully.", "পোশাক কেটে সফলভাবে দর্জির কাছে দেওয়া হয়েছে।")
            , ["ItemWaitingTailor"] = ("Item is waiting for tailor assignment.", "পোশাকটি দর্জি নিয়োগের অপেক্ষায় আছে।")
            , ["ConfirmCutAssignText"] = ("Mark the item as cut and assign it to this tailor?", "পোশাকটি কাটা হয়েছে হিসেবে চিহ্নিত করে এই দর্জির কাছে দেবেন?")
            , ["AssignedToTailorDashboard"] = ("The item has been assigned to the tailor's dashboard.", "পোশাকটি দর্জির ড্যাশবোর্ডে দেওয়া হয়েছে।")
            , ["ItemCutWithoutTailor"] = ("Mark the item as cut without assigning a tailor?", "দর্জি নিয়োগ না করে পোশাকটি কাটা হয়েছে হিসেবে চিহ্নিত করবেন?")
            , ["AbayaFrontLength"] = ("Abaya Length (Front)", "আবায়ার দৈর্ঘ্য (সামনে)")
            , ["AbayaBackLength"] = ("Abaya Length (Back)", "আবায়ার দৈর্ঘ্য (পেছনে)")
            , ["SleeveLength"] = ("Sleeve Length", "হাতার দৈর্ঘ্য")
            , ["ArmHoleWidth"] = ("Arm Hole Width", "বগলের প্রস্থ")
            , ["SleeveOpeningWidth"] = ("Sleeve Opening Width", "হাতার মুখের প্রস্থ")
            , ["ShoulderWidth"] = ("Shoulder Width", "কাঁধের প্রস্থ")
            , ["BodyChestWidth"] = ("Body / Chest Width", "শরীর / বুকের প্রস্থ")
            , ["BottomFlareWidth"] = ("Bottom / Flare Width", "নিচের / ঘেরের প্রস্থ")
            , ["WorkflowCreated"] = ("Initial workflow status assigned when the order was created.", "অর্ডার তৈরি করার সময় কাজের ধাপ নির্ধারণ করা হয়েছে।")
            , ["WorkflowUpdated"] = ("Workflow status assigned while the order was updated.", "অর্ডার আপডেট করার সময় কাজের ধাপ নির্ধারণ করা হয়েছে।")
            , ["FabricProcurementCompleted"] = ("Fabric procurement completed.", "কাপড় ক্রয়ের কাজ সম্পন্ন হয়েছে।")
            , ["AbayaFabricProcured"] = ("Abaya fabric procured.", "আবায়ার কাপড় কেনা হয়েছে।")
            , ["TailorAssignedAfterCutting"] = ("Tailor assigned after cutting was completed.", "কাটিং শেষ হওয়ার পর দর্জি নিয়োগ করা হয়েছে।")
            , ["TailorAssignedAtCutting"] = ("Tailor assigned at the time of cutting.", "কাটিংয়ের সময় দর্জি নিয়োগ করা হয়েছে।")
            , ["CuttingCompleted"] = ("Cutting completed.", "কাটিং সম্পন্ন হয়েছে।")
            , ["HandEmbAssigned"] = ("Hand embroiderer assigned.", "হাতের কাজের কর্মী নিয়োগ করা হয়েছে।")
            , ["HandEmbStarted"] = ("Hand embroiderer started the task.", "হাতের কাজের কর্মী কাজ শুরু করেছেন।")
            , ["HandEmbCompleted"] = ("Hand embroidery completed.", "হাতের কাজ সম্পন্ন হয়েছে।")
            , ["ItemReceivedAtShowroom"] = ("Item received at showroom.", "পোশাক শোরুমে গ্রহণ করা হয়েছে।")
            , ["ItemDelivered"] = ("Item delivered to customer.", "পোশাক ক্রেতাকে ডেলিভারি করা হয়েছে।")
            , ["TranslateWorkflowNoteFallback"] = ("", "")
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
            "TailorStartedStitching" => this["TailorStartedStitching"],
            "TailorCompletedStitching" => this["TailorCompletedStitching"],
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
