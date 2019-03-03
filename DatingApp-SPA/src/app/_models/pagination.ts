import { ApiResult } from './api-result';

export interface Pagination {
    currentPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
}

export class PaginatedResult<T> {
    result: ApiResult<T>;
    pagination: Pagination;
}
