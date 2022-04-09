using Domain.Entities;
using Domain.Models.Requests;
using FluentAssertions;
using Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitTests.AutoData;
using Xunit;

namespace UnitTests.Payment.Helpers
{
    public class StatementResponseHelpersTests
    {
        [Theory, AutoNSubstituteData]
        public void MapTransactions_ShouldMapCorrectly(
            int accountId,
            int trasactionId,
            decimal amount,
            int accountId2,
            int trasactionId2,
            decimal amount2)
        {
            var transactions = new List<Transacao>()
            {
                new Transacao
                {
                    DataTransacao = DateTime.Now,
                    IdConta = accountId,
                    IdTransacao =trasactionId,
                    Valor = amount
                },
                 new Transacao
                {
                    DataTransacao = DateTime.Now,
                    IdConta = accountId2,
                    IdTransacao = trasactionId2,
                    Valor = amount2
                }
            };

            var actual = StatementResponseHelpers.MapTransactions(transactions);

            actual.Should().NotBeNull();
            actual.Should().BeOfType<StatementResponse>();
            actual.Transacoes.Count().Should().Be(2);           
        }

        [Fact]
        public void MapTransactions_WhenTransactionListIsNull_ShouldReturnEmptyList()
        {
            var actual = StatementResponseHelpers.MapTransactions(null);

            actual.Should().NotBeNull();
            actual.Should().BeOfType<StatementResponse>();
            actual.Transacoes.Should().BeNull();
        }

        [Fact]
        public void MapTransactions_WhenTransactionListEmpty_ShouldReturnEmptyList()
        {
            var transaction = new List<Transacao>();
            var actual = StatementResponseHelpers.MapTransactions(transaction);

            actual.Should().NotBeNull();
            actual.Should().BeOfType<StatementResponse>();
            actual.Transacoes.Should().BeEmpty();
        }
    }
}
