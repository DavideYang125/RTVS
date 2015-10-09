using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.R.Host.Client;

namespace Microsoft.VisualStudio.R.Package.Repl.Session {
    internal sealed class RSessionEvaluation : IRSessionEvaluation {
        private readonly IRExpressionEvaluator _evaluator;
        private readonly TaskCompletionSource<object> _tcs;
        private readonly CancellationToken _ct;

        public IReadOnlyCollection<IRContext> Contexts { get; }
        public Task Task => _tcs.Task;

        public RSessionEvaluation(IReadOnlyCollection<IRContext> contexts, IRExpressionEvaluator evaluator, CancellationToken ct) {
            Contexts = contexts;
            _evaluator = evaluator;
            _tcs = new TaskCompletionSource<object>();
            _ct = ct;
            ct.Register(() => _tcs.TrySetCanceled());
        }

        public void Dispose() {
            _tcs.SetResult(null);
        }

        public Task<REvaluationResult> EvaluateAsync(string expression, bool reentrant) {
            return _evaluator.EvaluateAsync(expression, reentrant, _ct);
        }
    }
}