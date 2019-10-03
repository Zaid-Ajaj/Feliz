import { useEffect as useEffectReact } from 'react'

export const useEffect = getDisposable => {
    useEffectReact(() => {
        const disposable = getDisposable()
        return () => {
            disposable.Dispose();
        }
    })
}

export const useEffectWithDeps = (getDisposable, deps) => {
    useEffectReact(() => {
        const disposable = getDisposable()
        return () => {
            disposable.Dispose();
        }
    }, deps)
}