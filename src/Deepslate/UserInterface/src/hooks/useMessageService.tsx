import { useRef } from "react";

interface MessageService 
{
    sendPing(nonce : number): Promise<{nonce: number}>;
}

const useMessageService = (): MessageService => {
    const responseHandlers = useRef<Map<string, (response: object) => void>>(
        new Map()
    );

    const sendMessage = (type: string, data: object): Promise<object> => {
        return new Promise((resolve) => {
            const id = `${Date.now()}-${Math.random()}`;
            responseHandlers.current.set(id, resolve);
            const requestJson = JSON.stringify({ id, type, data });
            // @ts-expect-error Photino still uses this.
            window.external.sendMessage(requestJson);
            // @ts-expect-error Photino still uses this.
            window.external.receiveMessage((message) => {
                console.log(message);
                if (responseHandlers.current.has(id)) {
                    responseHandlers.current.get(id)!(JSON.parse(message).payload);
                    responseHandlers.current.delete(id);
                }
            });
        });
    };

    const sendPing = (nonce: number): Promise<{ nonce: number }> => {
        return sendMessage("ping", { nonce }) as Promise<{ nonce: number }>;
    }

    return { sendPing };
};

export default useMessageService;
