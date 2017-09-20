%{
**********************************************************************

Copyright (c) 2003-2017 ZeroC, Inc. All rights reserved.

This copy of Ice is licensed to you under the terms described in the
ICE_LICENSE file included in this distribution.

**********************************************************************
%}

classdef (Abstract) WrapperObject < handle
    methods
        function obj = WrapperObject(impl, type)
            %
            % If no type was supplied, we convert the class name into the prefix that we'll use for invoking
            % external C functions.
            %
            if nargin == 1
                type = strrep(class(obj), '.', '_'); % E.g., Ice.Communicator -> Ice_Communicator
            end
            obj.impl_ = impl;
            obj.type_ = type;
        end
    end
    methods(Hidden)
        function delete(obj)
            if ~isempty(obj.impl_)
                obj.call_('_release');
            end
        end
        function call_(obj, fn, varargin)
            name = strcat(obj.type_, '_', fn);
            ex = calllib('icematlab', name, obj.impl_, varargin{:});
            if ~isempty(ex)
                ex.throwAsCaller();
            end
        end
        function r = callWithResult_(obj, fn, varargin)
            name = strcat(obj.type_, '_', fn);
            result = calllib('icematlab', name, obj.impl_, varargin{:});
            if isempty(result)
                r = result;
            elseif ~isempty(result.exception)
                result.exception.throwAsCaller();
            else
                r = result.result;
            end
        end
        function callOnType_(obj, type, fn, varargin)
            name = strcat(type, '_', fn);
            ex = calllib('icematlab', name, obj.impl_, varargin{:});
            if ~isempty(ex)
                ex.throwAsCaller();
            end
        end
        function r = callOnTypeWithResult_(obj, type, fn, varargin)
            name = strcat(type, '_', fn);
            result = calllib('icematlab', name, obj.impl_, varargin{:});
            if isempty(result)
                r = result;
            elseif ~isempty(result.exception)
                result.exception.throwAsCaller();
            else
                r = result.result;
            end
        end
    end
    properties(Hidden,SetAccess=protected)
        impl_
        type_
    end
end