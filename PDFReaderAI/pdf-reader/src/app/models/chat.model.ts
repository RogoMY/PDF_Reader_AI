export interface Chat {
    id?: string;
    Title: string;
    Prompts: string[];
    Responses: string[];
    timeOfDiscussion: Date;
    FileName: string;
    FileContent: Uint8Array;
    FileMimeType: string;
    FileBase64Content?: string;


}
