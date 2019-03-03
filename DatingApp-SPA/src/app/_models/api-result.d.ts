import { ModelError } from './modal-error'
export declare class ApiResult<T> {
    statusCode: number;
    status: boolean;
    message: string;
    data: T;
    modelErrors: ModelError[];
}