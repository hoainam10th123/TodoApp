export interface Todo{
    id: number;
    name: string;
    description: string;
    createdDate: Date;
    startDate: Date;
    endDate: Date;
    status: Status;
    username: string;
    userId: number;
}

export enum Status{
    Done = 0,
    Cancel = 1,
    Doing = 2
}