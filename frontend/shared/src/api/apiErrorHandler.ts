export function extractApiError(err: any): string {
    if (err?.response?.data) {
        const data = err.response.data

        // Case 1: Validation errors { field: [message1, message2] }
        if (data.errors && typeof data.errors === 'object') {
            return Object.entries(data.errors)
                .map(([field, msgs]) => `${field}: ${(msgs as string[]).join(', ')}`)
                .join('\n')
        }

        // Case 2: Problem Details format (RFC 7807)
        if (data.detail || data.title) {
            return data.detail || data.title
        }

        // Default case
        return 'Validation failed'
    }

    // Network or unexpected error
    return err?.message || 'Unknown error occurred'
}
