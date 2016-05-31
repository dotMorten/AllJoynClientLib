//
// Copyright (c) 2015, Microsoft Corporation
//
// Permission to use, copy, modify, and/or distribute this software for any
// purpose with or without fee is hereby granted, provided that the above
// copyright notice and this permission notice appear in all copies.
//
// THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
// WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
// MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY
// SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
// WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
// ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF OR
// IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
//

#pragma once

#include "collection.h"
#include "ITypeDefinition.h"
#include "AllJoynTypeDefinition.h"
#include "AllJoynHelpers.h"

namespace DeviceProviders
{
    public ref class AllJoynMessageArgVariant sealed
    {
        DEBUG_LIFETIME_DECL(AllJoynMessageArgVariant);

    public:
        property ITypeDefinition^ TypeDefinition
        {
            ITypeDefinition^ get()
            {
                return m_typeDefinition;
            }
        }

        property Platform::Object^ Value
        {
            Platform::Object^ get()
            {
                return m_value;
            }
        }
		AllJoynMessageArgVariant(ITypeDefinition^ typeDefinition, Platform::Object^ value)
			: m_typeDefinition(typeDefinition), m_value(value)
		{

		}

    internal:
        AllJoynMessageArgVariant(_In_ PCSTR signature, _In_ Platform::Object^ value)
            : m_value(value)
        {
            DEBUG_LIFETIME_IMPL(AllJoynMessageArgVariant);

            auto typeDefinitions = AllJoynTypeDefinition::CreateTypeDefintions(signature);
            if (typeDefinitions && typeDefinitions->Size == 1)
            {
                m_typeDefinition = typeDefinitions->GetAt(0);
            }
        }

    private:
        ITypeDefinition^ m_typeDefinition;
        Platform::Object^ m_value;

    };
}