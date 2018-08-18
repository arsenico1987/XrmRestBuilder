// =====================================================================
//  This file is part of the Microsoft Dynamics CRM SDK code samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  This source code is intended only as a supplement to Microsoft
//  Development Tools and/or on-line documentation.  See these other
//  materials for detailed information regarding Microsoft code samples.
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
// =====================================================================
// <snippetJQueryRESTDataOperations.SDK.JQuery.js>
"undefined" == typeof SDK && (SDK = { __namespace: !0 }), SDK.JQuery = { _context: function () { if ("undefined" != typeof GetGlobalContext) return GetGlobalContext(); if ("undefined" != typeof Xrm) return Xrm.Page.context; throw new Error("Context is not available.") }, _getClientUrl: function () { var a = this._context().getClientUrl(); return a }, _ODataPath: function () { return this._getClientUrl() + "/XRMServices/2011/OrganizationData.svc/" }, _errorHandler: function (a) { return new Error("Error : " + a.status + ": " + a.statusText + ": " + JSON.parse(a.responseText).error.message.value) }, _dateReviver: function (a, b) { var c; return "string" == typeof b && (c = /Date\(([-+]?\d+)\)/.exec(b)) ? new Date(parseInt(b.replace("/Date(", "").replace(")/", ""), 10)) : b }, _parameterCheck: function (a, b) { if ("undefined" == typeof a || null === a) throw new Error(b) }, _stringParameterCheck: function (a, b) { if ("string" != typeof a) throw new Error(b) }, _callbackParameterCheck: function (a, b) { if ("function" != typeof a) throw new Error(b) }, createRecord: function (a, b, c, d) { this._parameterCheck(a, "SDK.JQuery.createRecord requires the object parameter."), this._stringParameterCheck(b, "SDK.JQuery.createRecord requires the type parameter is a string."), this._callbackParameterCheck(c, "SDK.JQuery.createRecord requires the successCallback is a function."), this._callbackParameterCheck(d, "SDK.JQuery.createRecord requires the errorCallback is a function."); var e = window.JSON.stringify(a); $.ajax({ type: "POST", contentType: "application/json; charset=utf-8", datatype: "json", url: this._ODataPath() + b + "Set", data: e, beforeSend: function (a) { a.setRequestHeader("Accept", "application/json") }, success: function (a, b, d) { c(a.d) }, error: function (a, b, c) { d(SDK.JQuery._errorHandler(a)) } }) }, retrieveRecord: function (a, b, c, d, e, f) { this._stringParameterCheck(a, "SDK.JQuery.retrieveRecord requires the id parameter is a string."), this._stringParameterCheck(b, "SDK.JQuery.retrieveRecord requires the type parameter is a string."), null != c && this._stringParameterCheck(c, "SDK.JQuery.retrieveRecord requires the select parameter is a string."), null != d && this._stringParameterCheck(d, "SDK.JQuery.retrieveRecord requires the expand parameter is a string."), this._callbackParameterCheck(e, "SDK.JQuery.retrieveRecord requires the successCallback parameter is a function."), this._callbackParameterCheck(f, "SDK.JQuery.retrieveRecord requires the errorCallback parameter is a function."); var g = ""; if (null != c || null != d) { if (g = "?", null != c) { var h = "$select=" + c; null != d && (h = h + "," + d), g += h } null != d && (g = g + "&$expand=" + d) } $.ajax({ type: "GET", contentType: "application/json; charset=utf-8", datatype: "json", url: this._ODataPath() + b + "Set(guid'" + a + "')" + g, beforeSend: function (a) { a.setRequestHeader("Accept", "application/json") }, success: function (a, b, c) { e(JSON.parse(c.responseText, SDK.JQuery._dateReviver).d) }, error: function (a, b, c) { f(SDK.JQuery._errorHandler(a)) } }) }, updateRecord: function (a, b, c, d, e) { this._stringParameterCheck(a, "SDK.JQuery.updateRecord requires the id parameter."), this._parameterCheck(b, "SDK.JQuery.updateRecord requires the object parameter."), this._stringParameterCheck(c, "SDK.JQuery.updateRecord requires the type parameter."), this._callbackParameterCheck(d, "SDK.JQuery.updateRecord requires the successCallback is a function."), this._callbackParameterCheck(e, "SDK.JQuery.updateRecord requires the errorCallback is a function."); var f = window.JSON.stringify(b); $.ajax({ type: "POST", contentType: "application/json; charset=utf-8", datatype: "json", data: f, url: this._ODataPath() + c + "Set(guid'" + a + "')", beforeSend: function (a) { a.setRequestHeader("Accept", "application/json"), a.setRequestHeader("X-HTTP-Method", "MERGE") }, success: function (a, b, c) { d() }, error: function (a, b, c) { e(SDK.JQuery._errorHandler(a)) } }) }, deleteRecord: function (a, b, c, d) { this._stringParameterCheck(a, "SDK.JQuery.deleteRecord requires the id parameter."), this._stringParameterCheck(b, "SDK.JQuery.deleteRecord requires the type parameter."), this._callbackParameterCheck(c, "SDK.JQuery.deleteRecord requires the successCallback is a function."), this._callbackParameterCheck(d, "SDK.JQuery.deleteRecord requires the errorCallback is a function."), $.ajax({ type: "POST", contentType: "application/json; charset=utf-8", datatype: "json", url: this._ODataPath() + b + "Set(guid'" + a + "')", beforeSend: function (a) { a.setRequestHeader("Accept", "application/json"), a.setRequestHeader("X-HTTP-Method", "DELETE") }, success: function (a, b, d) { c() }, error: function (a, b, c) { d(SDK.JQuery._errorHandler(a)) } }) }, retrieveMultipleRecords: function (a, b, c, d, e) { this._stringParameterCheck(a, "SDK.JQuery.retrieveMultipleRecords requires the type parameter is a string."), null != b && this._stringParameterCheck(b, "SDK.JQuery.retrieveMultipleRecords requires the options parameter is a string."), this._callbackParameterCheck(c, "SDK.JQuery.retrieveMultipleRecords requires the successCallback parameter is a function."), this._callbackParameterCheck(d, "SDK.JQuery.retrieveMultipleRecords requires the errorCallback parameter is a function."), this._callbackParameterCheck(e, "SDK.JQuery.retrieveMultipleRecords requires the OnComplete parameter is a function."); var f; null != b && (f = "?" != b.charAt(0) ? "?" + b : b), $.ajax({ type: "GET", contentType: "application/json; charset=utf-8", datatype: "json", url: this._ODataPath() + a + "Set" + f, beforeSend: function (a) { a.setRequestHeader("Accept", "application/json") }, success: function (b, f, g) { if (b && b.d && b.d.results) if (c(JSON.parse(g.responseText, SDK.JQuery._dateReviver).d.results), null != b.d.__next) { var h = b.d.__next.substring((SDK.JQuery._ODataPath() + a + "Set").length); SDK.JQuery.retrieveMultipleRecords(a, h, c, d, e) } else e() }, error: function (a, b, c) { d(SDK.JQuery._errorHandler(a)) } }) }, __namespace: !0 };